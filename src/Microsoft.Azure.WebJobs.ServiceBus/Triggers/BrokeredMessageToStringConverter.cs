// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Converters;
using Microsoft.Azure.ServiceBus;

namespace Microsoft.Azure.WebJobs.ServiceBus.Triggers
{
    internal class BrokeredMessageToStringConverter : IAsyncConverter<Message, string>
    {
        public Task<string> ConvertAsync(Message input, CancellationToken cancellationToken)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            try
            {
                return Task.FromResult(StrictEncodings.Utf8.GetString(input.Body));
            }
            catch (Exception exception)
            {
                string contentType = input.ContentType ?? "null";
                string msg = string.Format(CultureInfo.InvariantCulture, "The Message with ContentType '{0}' failed to deserialize to a string with the message: '{1}'",
                    contentType, exception.Message);

                throw new InvalidOperationException(msg, exception);
            }
        }
    }
}
