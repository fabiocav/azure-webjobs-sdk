// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.WebJobs.Host.Bindings
{
    /// <summary>
    /// Class containing helper methods for path binding
    /// </summary>
    internal static class BindingDataPathHelper
    {
        /// <summary>
        /// Convert a parameter value of supported type into path compatible string value.
        /// The set of supported types is limited to built-in signed/unsigned integer types, 
        /// strings, JToken, and Guid (which is translated in canonical form without curly braces).
        /// </summary>
        /// <param name="parameterValue">The parameter value to convert</param>
        /// <returns>Path compatible string representation of the given parameter or null if its type is not supported.</returns>
        public static string ConvertParameterValueToString(object parameterValue)
        {
            if (parameterValue != null)
            {
                switch (parameterValue)
                {
                    case string value:
                        return value;
                    case short value:
                        return value.ToString(CultureInfo.InvariantCulture);
                    case int value:
                        return value.ToString(CultureInfo.InvariantCulture);
                    case long value:
                        return value.ToString(CultureInfo.InvariantCulture);
                    case ushort value:
                        return value.ToString(CultureInfo.InvariantCulture);
                    case uint value:
                        return value.ToString(CultureInfo.InvariantCulture);
                    case ulong value:
                        return value.ToString(CultureInfo.InvariantCulture);
                    case char value:
                        return value.ToString();
                    case Byte value:
                        return value.ToString(CultureInfo.InvariantCulture);
                    case sbyte value:
                        return value.ToString(CultureInfo.InvariantCulture);
                    case Guid value:
                        return value.ToString();
                    case JToken value:
                        // Only accept primitive Json values. Don't accept complex objects. 
                        if (!(parameterValue is JContainer))
                        {
                            return parameterValue.ToString();
                        }
                        break;
                }
            }

            return null;
        }
    }
}
