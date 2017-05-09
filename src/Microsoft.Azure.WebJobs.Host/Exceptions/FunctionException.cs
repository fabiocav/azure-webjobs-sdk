// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Microsoft.Azure.WebJobs.Host
{
    /// <summary>
    /// Exception that is tied to a specific job method
    /// </summary>
    public class FunctionException : RecoverableException
    {
        /// <inheritdoc/>
        public FunctionException() : base()
        {
        }

        /// <inheritdoc/>
        public FunctionException(string message) : base(message)
        {
        }

        /// <inheritdoc/>
        public FunctionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="methodName">The name of the method in error.</param>
        /// <param name="innerException">The inner exception.</param>
        public FunctionException(string message, string methodName, Exception innerException)
            : base(message, innerException)
        {
            MethodName = methodName;
        }

        /// <summary>
        /// The name of the method in error.
        /// </summary>
        public string MethodName { get; private set; }
    }
}
