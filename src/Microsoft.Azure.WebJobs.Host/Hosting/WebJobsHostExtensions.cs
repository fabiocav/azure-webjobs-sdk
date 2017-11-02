using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Indexers;
using Microsoft.Azure.WebJobs.Host.Loggers;
using Microsoft.Azure.WebJobs.Host.Queues;
using Microsoft.Azure.WebJobs.Host.Timers;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Hosting
{
    public static class WebJobsHostExtensions
    {
        public static IHostBuilder ConfigureWebJobsHost(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IExtensionRegistry, DefaultExtensionRegistry>();
                services.AddSingleton<IConsoleProvider, DefaultConsoleProvider>();
                services.AddSingleton<ITypeLocator>(p => new DefaultTypeLocator(p.GetRequiredService<IConsoleProvider>().Out, p.GetRequiredService<IExtensionRegistry>()));
                services.AddSingleton<IConverterManager, ConverterManager>();
                services.AddSingleton<IWebJobsExceptionHandler, WebJobsExceptionHandler>();

                services.AddSingleton<IQueueConfiguration, JobHostQueuesConfiguration>();
                
                // TODO: Remove passing the service provider here.
                services.AddSingleton<IStorageAccountProvider>(p => new DefaultStorageAccountProvider(p));
                services.AddSingleton<StorageClientFactory, StorageClientFactory>();
                services.AddSingleton<INameResolver, DefaultNameResolver>();
                services.AddSingleton<IJobActivator>(p => DefaultJobActivator.Instance);
                services.AddSingleton<IFunctionResultAggregatorFactory, FunctionResultAggregatorFactory>();
                services.AddSingleton<IHostedService, JobHostService>();
            });

            return builder;
        }
    }
}
