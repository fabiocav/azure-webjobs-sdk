// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.ServiceBus.Bindings;
using Microsoft.Azure.WebJobs.ServiceBus.Triggers;
using Microsoft.Extensions.Options;

namespace Microsoft.Azure.WebJobs.ServiceBus.Config
{
    /// <summary>
    /// Extension configuration provider used to register ServiceBus triggers and binders
    /// </summary>
    public class ServiceBusExtensionConfig : IExtensionConfigProvider
    {
        private readonly INameResolver _nameResolver;
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly ServiceBusOptions _serviceBusConfig;
        private readonly IMessagingProvider _messagingProvider;

        /// <summary>
        /// Creates a new <see cref="ServiceBusExtensionConfig"/> instance.
        /// </summary>
        /// <param name="serviceBusConfig">The <see cref="ServiceBusOptions"></see> to use./></param>
        public ServiceBusExtensionConfig(IOptions<ServiceBusOptions> serviceBusConfig, IMessagingProvider messagingProvider, INameResolver nameResolver, IConnectionStringProvider connectionStringProvider)
        {
            _serviceBusConfig = serviceBusConfig.Value;
            _messagingProvider = messagingProvider;
            _nameResolver = nameResolver;
            _connectionStringProvider = connectionStringProvider;
        }

        /// <summary>
        /// Gets the <see cref="ServiceBusOptions"/>
        /// </summary>
        public ServiceBusOptions Config
        {
            get
            {
                return _serviceBusConfig;
            }
        }

        /// <inheritdoc />
        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            // register our trigger binding provider
            ServiceBusTriggerAttributeBindingProvider triggerBindingProvider = new ServiceBusTriggerAttributeBindingProvider(_nameResolver, _serviceBusConfig, _messagingProvider, _connectionStringProvider);
            context.AddBindingRule<ServiceBusTriggerAttribute>().BindToTrigger(triggerBindingProvider);

            // register our binding provider
            ServiceBusAttributeBindingProvider bindingProvider = new ServiceBusAttributeBindingProvider(_nameResolver, _serviceBusConfig, _connectionStringProvider);
            context.AddBindingRule<ServiceBusAttribute>().Bind(bindingProvider);
        }
    }
}
