// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs.Host.Converters;

namespace Microsoft.Azure.WebJobs.ServiceBus.Triggers
{
    internal class StringToBrokeredMessageConverterFactory
    {
        public static IConverter<string, Message> Create(Type parameterType)
        {
            if (parameterType == typeof(Message) || parameterType == typeof(string))
            {
                return new StringToTextBrokeredMessageConverter();
            }
            else if (parameterType == typeof(byte[]))
            {
                return new StringToBinaryBrokeredMessageConverter();
            }
            else
            {
                return new StringToJsonBrokeredMessageConverter();
            }
        }
    }
}
