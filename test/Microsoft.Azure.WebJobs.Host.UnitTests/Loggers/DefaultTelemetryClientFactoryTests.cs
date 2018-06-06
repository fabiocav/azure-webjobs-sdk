// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.Azure.WebJobs.Logging.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Microsoft.Azure.WebJobs.Host.UnitTests.Loggers
{
    public class DefaultTelemetryClientFactoryTests
    {
        [Fact]
        public void InitializeConfiguration_Configures()
        {
            var factory = new DefaultTelemetryClientFactory(string.Empty, null, null);
            var config = factory.InitializeConfiguration();

            // Verify Initializers
            Assert.Equal(3, config.TelemetryInitializers.Count);
            // These will throw if there are not exactly one
            config.TelemetryInitializers.OfType<WebJobsRoleEnvironmentTelemetryInitializer>().Single();
            config.TelemetryInitializers.OfType<WebJobsTelemetryInitializer>().Single();
            config.TelemetryInitializers.OfType<WebJobsSanitizingInitializer>().Single();

            // Verify Channel
            Assert.IsType<ServerTelemetryChannel>(config.TelemetryChannel);
        }


        [Fact]
        public void DepednencyInjectionConfiguration_Configures()
        {
            using (var host = new HostBuilder().AddApplicationInsights("some key", (c, l) => true).Build())
            {
                var config = host.Services.GetService<TelemetryConfiguration>();

                // Verify Initializers
                Assert.Equal(3, config.TelemetryInitializers.Count);
                // These will throw if there are not exactly one
                Assert.Single(config.TelemetryInitializers.OfType<WebJobsRoleEnvironmentTelemetryInitializer>());
                Assert.Single(config.TelemetryInitializers.OfType<WebJobsTelemetryInitializer>());
                Assert.Single(config.TelemetryInitializers.OfType<WebJobsSanitizingInitializer>());

                // Verify Channel
                Assert.IsType<ServerTelemetryChannel>(config.TelemetryChannel);

                var modules = host.Services.GetServices<ITelemetryModule>().ToList();

                // Verify Modules
                Assert.Equal(2, modules.Count);
                Assert.Single(modules.OfType<ServerTelemetryChannel>());
                Assert.Single(modules.OfType<QuickPulseTelemetryModule>());
                Assert.Same(config.TelemetryChannel, modules.OfType<ServerTelemetryChannel>());
                // Verify client
                var client = host.Services.GetService<TelemetryClient>();
                Assert.NotNull(client);
                Assert.True(client.Context.GetInternalContext().SdkVersion.StartsWith("webjobs"));

                // Verify provider
                Assert.NotNull(host.Services.GetService<ApplicationInsightsLoggerProvider>());
            }
        }
    }
}