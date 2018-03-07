// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Blobs;
using Microsoft.Azure.WebJobs.Host.Configuration;
using Microsoft.Azure.WebJobs.Host.Dispatch;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Indexers;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs.Host.Loggers;
using Microsoft.Azure.WebJobs.Host.Queues;
using Microsoft.Azure.WebJobs.Host.Timers;
using Microsoft.Azure.WebJobs.Host.Triggers;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.Azure.WebJobs.Hosting
{
    public static class WebJobsHostExtensions
    {
        public static IHostBuilder ConfigureWebJobsHost(this IHostBuilder builder)
        {
            return builder.ConfigureWebJobsHost(o => { });
        }

        public static IHostBuilder ConfigureWebJobsHost(this IHostBuilder builder, Action<JobHostOptions> configure)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.Configure(configure);

                // Temporary... this needs to be removed and JobHostConfiguration needs to have settings
                // moved to the appropriate options implementation and all services registered through DI
                services.AddSingleton(p => new JobHostConfiguration(p.GetRequiredService<ILoggerFactory>()));

                // A LOT of the service registrations below need to be cleaned up
                // maintaining some of the existing dependencies and model we previously had, 
                // but this should be reviewed as it can be improved.
                services.AddSingleton<IExtensionRegistry, DefaultExtensionRegistry>();
                services.AddSingleton<IExtensionTypeLocator, ExtensionTypeLocator>();
                services.AddSingleton<IConsoleProvider, DefaultConsoleProvider>();
                services.AddSingleton<SingletonManager>();
                services.AddSingleton<SharedQueueHandler>();
                services.AddSingleton<IFunctionExecutor, FunctionExecutor>();
                services.AddSingleton<IJobHostContextFactory, JobHostContextFactory>();
                services.AddSingleton<IFunctionInstanceLogger, FunctionInstanceLogger>();
                services.AddSingleton<IFunctionIndexProvider, FunctionIndexProvider>();
                services.AddSingleton<IBindingProviderFactory, DefaultBindingProvider>();
                services.AddSingleton<ISharedContextProvider, SharedContextProvider>();
                services.AddSingleton<IContextSetter<IMessageEnqueuedWatcher>>((p) => new ContextAccessor<IMessageEnqueuedWatcher>());
                services.AddSingleton<IContextSetter<IBlobWrittenWatcher>>((p) => new ContextAccessor<IBlobWrittenWatcher>());
                services.AddSingleton((p) => p.GetService<IContextSetter<IMessageEnqueuedWatcher>>() as IContextGetter<IMessageEnqueuedWatcher>);
                services.AddSingleton((p) => p.GetService<IContextSetter<IBlobWrittenWatcher>>() as IContextGetter<IBlobWrittenWatcher>);
                services.AddSingleton<LoggerProviderFactory>();
                services.AddSingleton<IFunctionOutputLoggerProvider>(p=>p.GetRequiredService<LoggerProviderFactory>().GetLoggerProvider<IFunctionOutputLoggerProvider>());
                services.AddSingleton<IFunctionInstanceLoggerProvider>(p=>p.GetRequiredService<LoggerProviderFactory>().GetLoggerProvider<IFunctionInstanceLoggerProvider>());
                services.AddSingleton<IHostInstanceLoggerProvider>(p=>p.GetRequiredService<LoggerProviderFactory>().GetLoggerProvider<IHostInstanceLoggerProvider>());
                services.AddSingleton<IDistributedLockManagerFactory, DefaultDistributedLockManagerFactory>();
                services.AddSingleton<IDistributedLockManager>(p => p.GetRequiredService<IDistributedLockManagerFactory>().Create());
                // TODO: FACAVAL FIX THIS
                services.AddSingleton<IHostIdProvider, FixedHostIdProvider>();

                services.AddSingleton<ITriggerBindingProvider>(p => DefaultTriggerBindingProvider.Create(p.GetRequiredService<INameResolver>(),
                    p.GetRequiredService<IStorageAccountProvider>(), p.GetRequiredService<IExtensionTypeLocator>(), p.GetRequiredService<IHostIdProvider>(),
                    p.GetRequiredService<IQueueConfiguration>(), p.GetRequiredService<IOptions<JobHostBlobsOptions>>(), p.GetRequiredService<IWebJobsExceptionHandler>(),
                    p.GetRequiredService<IContextSetter<IMessageEnqueuedWatcher>>(), p.GetRequiredService<IContextSetter<Host.Blobs.IBlobWrittenWatcher>>(),
                    p.GetRequiredService<ISharedContextProvider>(), p.GetRequiredService<IExtensionRegistry>(), p.GetRequiredService<SingletonManager>(),
                    p.GetRequiredService<ILoggerFactory>()));

                services.AddSingleton<ITypeLocator>(p => new DefaultTypeLocator(p.GetRequiredService<IConsoleProvider>().Out, p.GetRequiredService<IExtensionRegistry>()));
                services.AddSingleton<IConverterManager, ConverterManager>();
                services.AddSingleton<IWebJobsExceptionHandlerFactory, DefaultWebJobsExceptionHandlerFactory>();
                services.AddSingleton<IWebJobsExceptionHandler>(p => p.GetRequiredService<IWebJobsExceptionHandlerFactory>().Create(p.GetRequiredService<IHost>()));

                services.AddSingleton<IQueueConfiguration, JobHostQueuesConfiguration>();

                // TODO: Remove passing the service provider here.
                services.AddSingleton<IStorageAccountProvider>(p => new DefaultStorageAccountProvider(p));
                services.AddSingleton<StorageClientFactory>();
                services.AddSingleton<INameResolver, DefaultNameResolver>();
                services.AddSingleton<IJobActivator, DefaultJobActivator>();
                services.AddSingleton<IFunctionResultAggregatorFactory, FunctionResultAggregatorFactory>();

                // Options setup
                services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<JobHostOptions>, JobHostOptionsSetup>());

                services.RegisterBuiltInBindings();

                services.AddSingleton<IHostedService, JobHostService>();
                services.AddSingleton<IJobHost, JobHost>();
            });

            return builder;
        }

        public static IServiceCollection RegisterBuiltInBindings(this IServiceCollection services)
        {
            services.AddSingleton<Host.Tables.TableExtension>();
            services.AddSingleton<Host.Queues.Bindings.QueueExtension>();
            services.AddSingleton<Host.Blobs.Bindings.BlobExtensionConfig>();
            services.AddSingleton<Host.Blobs.Triggers.BlobTriggerExtensionConfig>();

            return services;
        }
    }
}
