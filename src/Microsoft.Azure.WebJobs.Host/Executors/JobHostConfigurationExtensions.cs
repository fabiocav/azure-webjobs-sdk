// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Blobs;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Host.Dispatch;
using Microsoft.Azure.WebJobs.Host.Indexers;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs.Host.Loggers;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Azure.WebJobs.Host.Queues;
using Microsoft.Azure.WebJobs.Host.Queues.Bindings;
using Microsoft.Azure.WebJobs.Host.Queues.Listeners;
using Microsoft.Azure.WebJobs.Host.Storage;
using Microsoft.Azure.WebJobs.Host.Storage.Blob;
using Microsoft.Azure.WebJobs.Host.Storage.Queue;
using Microsoft.Azure.WebJobs.Host.Tables;
using Microsoft.Azure.WebJobs.Host.Timers;
using Microsoft.Azure.WebJobs.Host.Triggers;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Host.Blobs.Triggers;

namespace Microsoft.Azure.WebJobs.Host.Executors
{
    internal static class JobHostConfigurationExtensions
    {
        // Static initialization. Returns a service provider with some new services initialized. 
        // The new services:
        // - can retrieve static config like binders and converters; but the listeners haven't yet started.
        // - can be flowed into the runtime initialization to get a JobHost spun up and running.
        // This is just static initialization and should not need to make any network calls, 
        // and so this method should not need to be async. 
        // This can be called multiple times on a config, which is why it returns a new ServiceProviderWrapper
        // instead of modifying the config.
        public static ServiceProviderWrapper CreateStaticServices(this JobHostConfiguration config)
        {
            var services = new ServiceProviderWrapper(config);

            //var nameResolver = services.GetService_<INameResolver>();
            //IWebJobsExceptionHandler exceptionHandler = services.GetService<IWebJobsExceptionHandler>();
            //IQueueConfiguration queueConfiguration = services.GetService<IQueueConfiguration>();

            //IStorageAccountProvider storageAccountProvider = services.GetService<IStorageAccountProvider>();
            //IBindingProvider bindingProvider = services.GetService<IBindingProvider>();
            //SingletonManager singletonManager = services.GetService<SingletonManager>();

            //IHostIdProvider hostIdProvider = services.GetService<IHostIdProvider>();
            //var hostId = config.HostId;
            //if (hostId != null)
            //{
            //    hostIdProvider = new FixedHostIdProvider(hostId);
            //}

            //// Need a deferred getter since the IFunctionIndexProvider service isn't created until later. 
            //Func<IFunctionIndexProvider> deferredGetter = () => services.GetService<IFunctionIndexProvider>();
            ////if (hostIdProvider == null)
            ////{
            ////    hostIdProvider = new DynamicHostIdProvider(storageAccountProvider, deferredGetter);
            ////}
            //services.AddService<IHostIdProvider>(hostIdProvider);

            //AzureStorageDeploymentValidator.Validate();

            //ContextAccessor<IMessageEnqueuedWatcher> messageEnqueuedWatcherAccessor = new ContextAccessor<IMessageEnqueuedWatcher>();
            //services.AddService(messageEnqueuedWatcherAccessor);
            //ContextAccessor<IBlobWrittenWatcher> blobWrittenWatcherAccessor = new ContextAccessor<IBlobWrittenWatcher>();
            //services.AddService(blobWrittenWatcherAccessor);
            //ISharedContextProvider sharedContextProvider = new SharedContextProvider();

            // Add built-in extensions 
            //var metadataProvider = new JobHostMetadataProvider(deferredGetter);
            //metadataProvider.AddAttributesFromAssembly(typeof(TableAttribute).Assembly);

            //var converterManager = (ConverterManager)config.ConverterManager;

            //var exts = config.GetExtensions();
            //bool builtinsAdded = exts.GetExtensions<IExtensionConfigProvider>().OfType<TableExtension>().Any();
            //if (!builtinsAdded)
            //{
            //    config.AddExtension(new TableExtension());
            //    config.AddExtension(new QueueExtension());
            //    config.AddExtension(new Blobs.Bindings.BlobExtensionConfig());
            //    config.AddExtension(new BlobTriggerExtensionConfig());
            //}

            //ExtensionConfigContext context = new ExtensionConfigContext
            //{
            //    Config = config,
            //    PerHostServices = services
            //};
            //InvokeExtensionConfigProviders(context);

            // After this point, all user configuration has been set. 

            //IExtensionRegistry extensions = services.GetExtensions();
            //services.AddService<SharedQueueHandler>(new SharedQueueHandler(storageAccountProvider, hostIdProvider, exceptionHandler,
            //                                        config.LoggerFactory, queueConfiguration, sharedContextProvider, messageEnqueuedWatcherAccessor));

            //metadataProvider.Initialize(bindingProvider, converterManager, exts);
            //services.AddService<IJobHostMetadataProvider>(metadataProvider);

            return services;
        }

        //private static void InvokeExtensionConfigProviders(ExtensionConfigContext context)
        //{
        //    IExtensionRegistry extensions = context.Config.GetExtensions();

        //    IEnumerable<IExtensionConfigProvider> configProviders = extensions.GetExtensions(typeof(IExtensionConfigProvider)).Cast<IExtensionConfigProvider>();
        //    foreach (IExtensionConfigProvider configProvider in configProviders)
        //    {
        //        context.Current = configProvider;
        //        configProvider.Initialize(context);
        //    }
        //    context.ApplyRules();
        //}

        // When running in Azure Web Sites, write out a manifest file. This manifest file is read by
        // the Kudu site extension to provide custom behaviors for SDK jobs


        #region Backwards compat shim for ExtensionLocator
        // We can remove this when we fix https://github.com/Azure/azure-webjobs-sdk/issues/995

        // TODO: FACAVAL: The issue mentioned above has been closed as resolved. Is this still required?
        // create IConverterManager adapters to any legacy ICloudBlobStreamBinder<T>. 
        //static void AddStreamConverters(IExtensionTypeLocator extensionTypeLocator, ConverterManager cm)
        //{
        //    if (extensionTypeLocator == null)
        //    {
        //        return;
        //    }

        //    foreach (var type in extensionTypeLocator.GetCloudBlobStreamBinderTypes())
        //    {
        //        var instance = Activator.CreateInstance(type);

        //        var bindingType = Blobs.CloudBlobStreamObjectBinder.GetBindingValueType(type);
        //        var method = typeof(JobHostConfigurationExtensions).GetMethod("AddAdapter", BindingFlags.Static | BindingFlags.NonPublic);
        //        method = method.MakeGenericMethod(bindingType);
        //        method.Invoke(null, new object[] { cm, instance });
        //    }
        //}

        //static void AddAdapter<T>(ConverterManager cm, ICloudBlobStreamBinder<T> x)
        //{
        //    cm.AddExactConverter<Stream, T>(stream => x.ReadFromStreamAsync(stream, CancellationToken.None).Result);

        //    cm.AddExactConverter<ApplyConversion<T, Stream>, object>(pair =>
        //    {
        //        T value = pair.Value;
        //        Stream stream = pair.Existing;
        //        x.WriteToStreamAsync(value, stream, CancellationToken.None).Wait();
        //        return null;
        //    });
        //}
        #endregion
    }
}