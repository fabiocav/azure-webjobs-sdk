// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
#if !NETSTANDARD2_0
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.ApplicationInsights.WindowsServer;
using Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
#endif
using Microsoft.Extensions.Logging;

namespace Microsoft.Azure.WebJobs.Logging.ApplicationInsights
{
    /// <summary>
    /// Creates a <see cref="TelemetryClient"/> for use by the <see cref="ApplicationInsightsLogger"/>. 
    /// </summary>
    public class DefaultTelemetryClientFactory : ITelemetryClientFactory
    {
        private readonly string _instrumentationKey;
#if !NETSTANDARD2_0
        private readonly SamplingPercentageEstimatorSettings _samplingSettings;
        private QuickPulseTelemetryModule _quickPulseModule;
        private PerformanceCollectorModule _perfModule;
#endif
        private TelemetryConfiguration _config;
        private bool _disposed;
        private Func<string, LogLevel, bool> _filter;

#if !NETSTANDARD2_0
        /// <summary>
        /// Instantiates an instance.
        /// </summary>
        /// <param name="instrumentationKey">The Application Insights instrumentation key.</param>
        /// <param name="samplingSettings">The <see cref="SamplingPercentageEstimatorSettings"/> to use for configuring adaptive sampling. If null, sampling is disabled.</param>
        /// <param name="filter"></param>
        public DefaultTelemetryClientFactory(string instrumentationKey, SamplingPercentageEstimatorSettings samplingSettings, Func<string, LogLevel, bool> filter)
        {
            _instrumentationKey = instrumentationKey;
            _samplingSettings = samplingSettings;
            _filter = filter;
        }
#else
        /// <summary>
        /// Instantiates an instance.
        /// </summary>
        /// <param name="instrumentationKey">The Application Insights instrumentation key.</param>
        public DefaultTelemetryClientFactory(string instrumentationKey, Func<string, LogLevel, bool> filter)
        {
            _instrumentationKey = instrumentationKey;
            _filter = filter;
        }
#endif
        /// <summary>
        /// Creates a <see cref="TelemetryClient"/>. 
        /// </summary>
        /// <returns>The <see cref="TelemetryClient"/> instance.</returns>
        public virtual TelemetryClient Create()
        {
            _config = InitializeConfiguration();

            TelemetryClient client = new TelemetryClient(_config);

            string assemblyVersion = GetAssemblyFileVersion(typeof(JobHost).Assembly);
            client.Context.GetInternalContext().SdkVersion = $"webjobs: {assemblyVersion}";

            return client;
        }

        internal static string GetAssemblyFileVersion(Assembly assembly)
        {
            AssemblyFileVersionAttribute fileVersionAttr = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
            return fileVersionAttr?.Version ?? LoggingConstants.Unknown;
        }

        internal TelemetryConfiguration InitializeConfiguration()
        {
            TelemetryConfiguration config = new TelemetryConfiguration()
            {
                InstrumentationKey = _instrumentationKey
            };

            AddInitializers(config);

            // TODO: FACAVAL - working with brettsam to understand impact, reach out to the team and find alternatives
#if !NETSTANDARD2_0
            // Plug in Live stream and adaptive sampling
            QuickPulseTelemetryProcessor processor = null;
            TelemetryProcessorChainBuilder builder = config.TelemetryProcessorChainBuilder
                .Use((next) =>
                {
                    processor = new QuickPulseTelemetryProcessor(next);
                    return processor;
                })
                .Use((next) =>
                {
                    return new FilteringTelemetryProcessor(_filter, next);
                });

            if (_samplingSettings != null)
            {
                builder.Use((next) =>
                {
                    return new AdaptiveSamplingTelemetryProcessor(_samplingSettings, null, next);
                });
            }
            builder.Build();

            _quickPulseModule = CreateQuickPulseTelemetryModule();
            _quickPulseModule.Initialize(config);
            _quickPulseModule.RegisterTelemetryProcessor(processor);

            // Plug in perf counters
            _perfModule = new PerformanceCollectorModule();
            _perfModule.Initialize(config);

            // Configure the TelemetryChannel
            ITelemetryChannel channel = CreateTelemetryChannel();

            // call Initialize if available
            ITelemetryModule module = channel as ITelemetryModule;
            if (module != null)
            {
                module.Initialize(config);
            }

            config.TelemetryChannel = channel;
#endif

            return config;
        }

        /// <summary>
        /// Creates the <see cref="ITelemetryChannel"/> to be used by the <see cref="TelemetryClient"/>. If this channel
        /// implements <see cref="ITelemetryModule"/> as well, <see cref="ITelemetryModule.Initialize(TelemetryConfiguration)"/> will
        /// automatically be called.
        /// </summary>
        /// <returns>The <see cref="ITelemetryChannel"/></returns>
        protected virtual ITelemetryChannel CreateTelemetryChannel()
        {
#if !NETSTANDARD2_0
            return new ServerTelemetryChannel();
#else
            throw new NotSupportedException();
#endif
        }

        internal static void AddInitializers(TelemetryConfiguration config)
        {
#if !NETSTANDARD2_0
            // This picks up the RoleName from the server
            config.TelemetryInitializers.Add(new AzureWebAppRoleEnvironmentTelemetryInitializer());
#endif
            // This applies our special scope properties and gets RoleInstance name
            config.TelemetryInitializers.Add(new WebJobsTelemetryInitializer());
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the instance.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                // TelemetryConfiguration.Dispose will dispose the Channel and the TelemetryProcessors
                // registered with the TelemetryProcessorChainBuilder.
                _config?.Dispose();
#if !NETSTANDARD2_0
                _perfModule?.Dispose();
                _quickPulseModule?.Dispose();
#endif
                _disposed = true;
            }
        }
    }
}
