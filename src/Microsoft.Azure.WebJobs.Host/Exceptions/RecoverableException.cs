// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;
using Microsoft.Extensions.Logging;

namespace Microsoft.Azure.WebJobs.Host
{
    /// <summary>
    /// A recoverable exception, i.e. can be handled.
    /// </summary>
    public class RecoverableException : Exception
    {
        /// <inheritdoc/>
        public RecoverableException() : base()
        {
        }

        /// <inheritdoc/>
        public RecoverableException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public RecoverableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the exception should be treated
        /// as handled.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Tries to recover by propagating exception through trace pipeline.
        /// Recovers if exception is handled by trace pipeline, else throws.
        /// </summary>
        /// <param name="trace"></param>
        /// <param name="logger"></param>
        internal void TryRecover(TraceWriter trace, ILogger logger)
        {
            if (trace == null)
            {
                throw this;
            }

            trace.Error(Message, this);
            logger?.LogError(0, this, Message);
            if (!Handled)
            {
                throw this;
            }
        }
    }
}
