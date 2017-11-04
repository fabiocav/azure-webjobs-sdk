// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;

namespace SampleHost
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureWebJobsHost(o =>
                {
                    // Example setting options properties:
                    o.HostId = "testhostid";
                })
                .ConfigureAppConfiguration(config =>
                {
                    // Adding command line as a configuration source
                    config.AddCommandLine(args);
                })
                .UseConsoleLifetime();

            var jobHost = builder.Build();

            await jobHost.RunAsync();
        }

        private static void CheckAndEnableAppInsights(JobHostConfiguration config)
        {
            // If AppInsights is enabled, build up a LoggerFactory
            string instrumentationKey = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY");
            if (!string.IsNullOrEmpty(instrumentationKey))
            {
                var filter = new LogCategoryFilter();
                filter.DefaultLevel = LogLevel.Debug;
                filter.CategoryLevels[LogCategories.Results] = LogLevel.Debug;
                filter.CategoryLevels[LogCategories.Aggregator] = LogLevel.Debug;

                // Adjust the LogLevel for a specific Function.
                filter.CategoryLevels[LogCategories.CreateFunctionCategory(nameof(Functions.ProcessWorkItem))] = LogLevel.Debug;

                config.LoggerFactory = new LoggerFactory()
                    .AddApplicationInsights(instrumentationKey, filter.Filter)
                    .AddConsole(filter.Filter);
            }
        }
    }
}
