// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.ServiceBus;
using System.Reflection;

namespace Microsoft.Azure.WebJobs.ServiceBus.Triggers
{
    internal class BrokeredMessageValueProvider : IValueProvider
    {
        private readonly object _value;
        private readonly Type _valueType;
        private readonly string _invokeString;

        private BrokeredMessageValueProvider(object value, Type valueType, string invokeString)
        {
            if (value != null && !valueType.GetTypeInfo().IsAssignableFrom(value.GetType()))
            {
                throw new InvalidOperationException("value is not of the correct type.");
            }

            _value = value;
            _valueType = valueType;
            _invokeString = invokeString;
        }

        public Type Type
        {
            get { return _valueType; }
        }

        public Task<object> GetValueAsync()
        {
            return Task.FromResult(_value);
        }

        public string ToInvokeString()
        {
            return _invokeString;
        }

        public static async Task<BrokeredMessageValueProvider> CreateAsync(Message clone, object value,
            Type valueType, CancellationToken cancellationToken)
        {
            string invokeString = CreateInvokeString(clone, cancellationToken);
            return new BrokeredMessageValueProvider(value, valueType, invokeString);
        }

        private static string CreateInvokeString(Message clonedMessage,
            CancellationToken cancellationToken)
        {
            switch (clonedMessage.ContentType)
            {
                case ContentTypes.ApplicationJson:
                case ContentTypes.TextPlain:
                    return GetText(clonedMessage, cancellationToken);
                case ContentTypes.ApplicationOctetStream:
                    return GetBase64String(clonedMessage, cancellationToken);
                default:
                    return GetBytesLength(clonedMessage);
            }
        }

        private static string GetBase64String(Message clonedMessage,
            CancellationToken cancellationToken)
        {
            return Convert.ToBase64String(clonedMessage.Body);
        }

        private static string GetBytesLength(Message clonedMessage)
        {
            return string.Format(CultureInfo.InvariantCulture, "byte[{0}]", clonedMessage.Body.Length);
        }

        private static string GetText(Message clonedMessage,
            CancellationToken cancellationToken)
        {
            try
            {
                return StrictEncodings.Utf8.GetString(clonedMessage.Body);
            }
            catch (DecoderFallbackException)
            {
                return "byte[" + clonedMessage.Body.Length + "]";
            }
        }
    }
}
