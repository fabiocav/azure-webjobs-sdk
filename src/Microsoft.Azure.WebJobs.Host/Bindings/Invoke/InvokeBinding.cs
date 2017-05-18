// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Reflection;

namespace Microsoft.Azure.WebJobs.Host.Bindings.Invoke
{
    internal static class InvokeBinding
    {
        public static IBinding Create(string parameterName, Type parameterType)
        {
            if (parameterType.IsByRef)
            {
                return null;
            }

            TypeInfo parameterTypeInfo = parameterType.GetTypeInfo();
            if (parameterTypeInfo.ContainsGenericParameters)
            { 
                return null; 
            }

            Type genericTypeDefinition;

            if (!parameterTypeInfo.IsValueType)
            {
                genericTypeDefinition = typeof(ClassInvokeBinding<>);
            }
            else
            {
                genericTypeDefinition = typeof(StructInvokeBinding<>);
            }

            Type genericType = genericTypeDefinition.MakeGenericType(parameterType);
            return (IBinding)Activator.CreateInstance(genericType, parameterName);
        }
    }
}
