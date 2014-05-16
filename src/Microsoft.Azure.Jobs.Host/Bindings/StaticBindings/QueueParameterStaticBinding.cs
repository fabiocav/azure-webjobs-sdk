﻿using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Azure.Jobs.Host.Protocols;

namespace Microsoft.Azure.Jobs
{
    internal class QueueParameterStaticBinding : ParameterStaticBinding
    {
        // Is this enqueue or dequeue?
        public bool IsInput { get; set; }

        // Route params produced from this queue. 
        // This likely corresponds to simply properties on the QueueInput Parameter type.
        public string[] Params { get; set; }

        [JsonIgnore]
        private string _queueName;

        public string QueueName
        {
            get
            {
                return _queueName;
            }
            set
            {
                string name = value.ToLowerInvariant(); // must be lowercase. coerce here to be nice.
                QueueClient.ValidateQueueName(name);
                this._queueName = name;
            }
        }

        public override IEnumerable<string> ProducedRouteParameters
        {
            get
            {
                return Params ?? new string[0];
            }
        }

        public override ParameterRuntimeBinding Bind(IRuntimeBindingInputs inputs)
        {
            string invokeString = null;
            if (this.IsInput)
            {
                ITriggerNewQueueMessage trigger = inputs as ITriggerNewQueueMessage;

                if (trigger == null)
                {
                    throw new InvalidOperationException("Direct calls are not supported for QueueInput methods.");
                }

                invokeString = trigger.QueueMessageInput.AsString;
            }
            return this.BindFromInvokeString(inputs, invokeString);
        }

        public override ParameterRuntimeBinding BindFromInvokeString(IRuntimeBindingInputs inputs, string invokeString)
        {
            if (this.IsInput)
            {
                return new QueueInputParameterRuntimeBinding { Name = Name, Content = invokeString };
            }
            else
            {
                string queueName;

                if (string.IsNullOrEmpty(invokeString))
                {
                    queueName = _queueName;
                }
                else
                {
                    queueName = invokeString;
                }

                return new QueueOutputParameterRuntimeBinding
                {
                    Name = Name,
                    QueueOutput = new CloudQueueDescriptor
                    {
                        AccountConnectionString = inputs.AccountConnectionString,
                        QueueName = queueName
                    }
                };
            }
        }

        public override ParameterDescriptor ToParameterDescriptor()
        {
            return new QueueParameterDescriptor
            {
                QueueName = QueueName,
                IsInput = IsInput
            };
        }
    }
}
