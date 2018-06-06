// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Azure.WebJobs.Logging.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Logging
{
    /// <summary>
    /// Extensions for ApplicationInsights configurationon an <see cref="IHostBuilder"/>. 
    /// </summary>
    public static class ApplicationInsightsLoggerExtensions
    {
        /// <summary>
        /// Registers Application Insights and <see cref="ApplicationInsightsLoggerProvider"/> with an <see cref="IHostBuilder"/>.
        /// </summary>
        /// <param name="builder">The host builder.</param>
        /// <param name="instrumentationKey">The Application Insights instrumentation key.</param>
        /// <returns>A <see cref="IHostBuilder"/> for chaining additional operations.</returns>
        public static IHostBuilder AddApplicationInsights(this IHostBuilder builder, string instrumentationKey)
        {
            return AddApplicationInsights(builder, instrumentationKey, (_, level) => level > LogLevel.Debug);
        }

        /// <summary>
        /// Registers Application Insights and <see cref="ApplicationInsightsLoggerProvider"/> with an <see cref="IHostBuilder"/>.
        /// </summary>
        /// <param name="builder">The host builder.</param>
        /// <param name="instrumentationKey">The Application Insights instrumentation key.</param>
        /// <param name="filter">A filter that returns true if a message with the specified <see cref="LogLevel"/>
        /// and category should be logged. You can use <see cref="LogCategoryFilter.Filter(string, LogLevel)"/>
        /// or write a custom filter.</param>
        /// <returns>A <see cref="IHostBuilder"/> for chaining additional operations.</returns>
        public static IHostBuilder AddApplicationInsights(this IHostBuilder builder, string instrumentationKey, Func<string, LogLevel, bool> filter)
        {
            if (string.IsNullOrEmpty(instrumentationKey))
            {
                return builder;
            }

            builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<ITelemetryInitializer, WebJobsRoleEnvironmentTelemetryInitializer>();
                services.AddSingleton<ITelemetryInitializer, WebJobsTelemetryInitializer>();
                services.AddSingleton<ITelemetryInitializer, WebJobsSanitizingInitializer>();
                services.AddSingleton<ITelemetryModule, QuickPulseTelemetryModule>();

                ServerTelemetryChannel serverChannel = new ServerTelemetryChannel();
                services.AddSingleton<ITelemetryModule>(serverChannel);
                services.AddSingleton<ITelemetryChannel>(serverChannel);
                services.AddSingleton<TelemetryConfiguration>(provider =>
                {
                    ITelemetryChannel channel = provider.GetService<ITelemetryChannel>();
                    TelemetryConfiguration config = new TelemetryConfiguration(instrumentationKey, channel);
                    
                    foreach (ITelemetryInitializer initializer in provider.GetServices<ITelemetryInitializer>())
                    {
                        config.TelemetryInitializers.Add(initializer);
                    }

                    QuickPulseTelemetryModule quickPulseModule = null;
                    foreach (ITelemetryModule module in provider.GetServices<ITelemetryModule>())
                    {
                        if (module is QuickPulseTelemetryModule telemetryModule)
                        {
                            quickPulseModule = telemetryModule;
                        }
                        module.Initialize(config);
                    }

                    QuickPulseTelemetryProcessor processor = null;
                    config.TelemetryProcessorChainBuilder
                        .Use((next) =>
                        {
                            processor = new QuickPulseTelemetryProcessor(next);
                            return processor;
                        })
                        .Use((next) => new FilteringTelemetryProcessor(filter, next));

                    SamplingPercentageEstimatorSettings samplingSettings =
                        provider.GetService<IOptions<SamplingPercentageEstimatorSettings>>()?.Value;

                    if (samplingSettings != null)
                    {
                        config.TelemetryProcessorChainBuilder.Use((next) =>
                            new AdaptiveSamplingTelemetryProcessor(samplingSettings, null, next));
                    }

                    config.TelemetryProcessorChainBuilder.Build();
                    quickPulseModule?.RegisterTelemetryProcessor(processor);

                    return config;
                });
                services.AddSingleton<TelemetryClient>(provider =>
                {
                    TelemetryConfiguration configuration = provider.GetService<TelemetryConfiguration>();
                    TelemetryClient client = new TelemetryClient(configuration);

                    string assemblyVersion = GetAssemblyFileVersion(typeof(JobHost).Assembly);
                    client.Context.GetInternalContext().SdkVersion = $"webjobs: {assemblyVersion}";

                    return client;
                });

                services.AddSingleton<ILoggerProvider, ApplicationInsightsLoggerProvider>();
            });

            return builder;
        }

        internal static string GetAssemblyFileVersion(Assembly assembly)
        {
            AssemblyFileVersionAttribute fileVersionAttr = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
            return fileVersionAttr?.Version ?? LoggingConstants.Unknown;
        }

        /// <summary>
        /// Registers an <see cref="ApplicationInsightsLoggerProvider"/> with an <see cref="ILoggerFactory"/>.
        /// </summary>
        /// <param name="loggerFactory">The factory.</param>
        /// <param name="instrumentationKey">The Application Insights instrumentation key.</param>
        /// <param name="filter">A filter that returns true if a message with the specified <see cref="LogLevel"/>
        /// and category should be logged. You can use <see cref="LogCategoryFilter.Filter(string, LogLevel)"/>
        /// or write a custom filter.</param>
        /// <returns>A <see cref="ILoggerFactory"/> for chaining additional operations.</returns>
        [Obsolete("Use 'AddApplicationInsights' IHostBuilder extension method instead.", true)]
        public static ILoggerFactory AddApplicationInsights(
            this ILoggerFactory loggerFactory,
            string instrumentationKey,
            Func<string, LogLevel, bool> filter)
        {
            ITelemetryClientFactory defaultFactory = new DefaultTelemetryClientFactory(instrumentationKey,
                new SamplingPercentageEstimatorSettings(), filter);

            return AddApplicationInsights(loggerFactory, defaultFactory);
        }

        /// <summary>
        /// Registers an <see cref="ApplicationInsightsLoggerProvider"/> with an <see cref="ILoggerFactory"/>.
        /// </summary>
        /// <param name="loggerFactory">The factory.</param>        
        /// <param name="telemetryClientFactory">The factory to use when creating the <see cref="TelemetryClient"/> </param>
        /// <returns>A <see cref="ILoggerFactory"/> for chaining additional operations.</returns>
        [Obsolete("Use 'AddApplicationInsights' IHostBuilder extension method instead.", true)]
        public static ILoggerFactory AddApplicationInsights(
            this ILoggerFactory loggerFactory,
            ITelemetryClientFactory telemetryClientFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            // Note: LoggerFactory calls Dispose() on all registered providers.
            loggerFactory.AddProvider(new ApplicationInsightsLoggerProvider(telemetryClientFactory));

            return loggerFactory;
        }

        [Obsolete("Use 'AddApplicationInsights' IHostBuilder extension method instead.", true)]
        public static ILoggingBuilder AddApplicationInsights(this ILoggingBuilder builder, ITelemetryClientFactory telemetryClientFactory)
        {
            return builder.AddProvider(new ApplicationInsightsLoggerProvider(telemetryClientFactory));
        }
    }
}