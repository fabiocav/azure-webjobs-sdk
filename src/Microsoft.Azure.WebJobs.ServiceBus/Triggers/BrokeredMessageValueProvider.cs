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

namespace Microsoft.Azure.WebJobs.ServiceBus.Triggers
{
    internal class BrokeredMessageValueProvider : IValueProvider
    {
        private readonly object _value;
        private readonly Type _valueType;
        private readonly string _invokeString;

        private BrokeredMessageValueProvider(object value, Type valueType, string invokeString)
        {
            if (value != null && !valueType.IsAssignableFrom(value.GetType()))
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
            string invokeString = await CreateInvokeStringAsync(clone, cancellationToken);
            return new BrokeredMessageValueProvider(value, valueType, invokeString);
        }

        private static Task<string> CreateInvokeStringAsync(Message clonedMessage,
            CancellationToken cancellationToken)
        {
            switch (clonedMessage.ContentType)
            {
                case ContentTypes.ApplicationJson:
                case ContentTypes.TextPlain:
                    return GetTextAsync(clonedMessage, cancellationToken);
                case ContentTypes.ApplicationOctetStream:
                    return GetBase64String(clonedMessage, cancellationToken);
                default:
                    return GetBytesLengthAsync(clonedMessage);
            }
        }

        private static string GetBase64String(Message clonedMessage,
            CancellationToken cancellationToken)
        {
            return Convert.ToBase64String(clonedMessage.Body);
        }

        private static Task<string> GetBytesLengthAsync(Message clonedMessage)
        {
            long length;
            using (Stream inputStream = clonedMessage.GetBody<Stream>())
            {
                if (inputStream == null)
                {
                    return Task.FromResult<string>(null);
                }

                length = inputStream.Length;
            }

            string description = string.Format(CultureInfo.InvariantCulture, "byte[{0}]", length);

            return Task.FromResult(description);
        }

        private static async Task<string> GetTextAsync(Message clonedMessage,
            CancellationToken cancellationToken)
        {
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (Stream inputStream = clonedMessage.GetBody<Stream>())
                {
                    if (inputStream == null)
                    {
                        return null;
                    }

                    const int DefaultBufferSize = 4096;
                    await inputStream.CopyToAsync(outputStream, DefaultBufferSize, cancellationToken);
                    byte[] bytes = outputStream.ToArray();

                    try
                    {
                        return StrictEncodings.Utf8.GetString(bytes);
                    }
                    catch (DecoderFallbackException)
                    {
                        return "byte[" + bytes.Length + "]";
                    }
                }
            }
        }
    }
}
