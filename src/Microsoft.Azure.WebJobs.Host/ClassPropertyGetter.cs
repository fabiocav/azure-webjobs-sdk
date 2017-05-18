// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Reflection;

namespace Microsoft.Azure.WebJobs.Host
{
    internal class ClassPropertyGetter<TDeclaring, TProperty> : IPropertyGetter<TDeclaring, TProperty>
        where TDeclaring : class
    {
        private readonly PropertyGetterDelegate _getter;

        private ClassPropertyGetter(PropertyGetterDelegate getter)
        {
            Debug.Assert(getter != null);
            _getter = getter;
        }

        private delegate TProperty PropertyGetterDelegate(TDeclaring instance);

        public TProperty GetValue(TDeclaring instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return _getter.Invoke(instance);
        }

        public static ClassPropertyGetter<TDeclaring, TProperty> Create(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            if (typeof(TDeclaring) != property.DeclaringType)
            {
                throw new ArgumentException("The property's DeclaringType must exactly match TDeclaring.", "property");
            }

            if (typeof(TProperty) != property.PropertyType)
            {
                throw new ArgumentException("The property's PropertyType must exactly match TProperty.", "property");
            }

            if (!property.CanRead)
            {
                throw new ArgumentException("The property must be readable.", "property");
            }

            if (property.GetIndexParameters().Length != 0)
            {
                throw new ArgumentException("The property must not have index parameters.", "property");
            }

            MethodInfo getMethod = property.GetMethod;
            Debug.Assert(getMethod != null);

            if (getMethod.IsStatic)
            {
                throw new ArgumentException("The property must not be static.", "property");
            }

            Debug.Assert(getMethod.DeclaringType == typeof(TDeclaring));
            Debug.Assert(!getMethod.DeclaringType.GetTypeInfo().IsValueType);
            Debug.Assert(getMethod.GetParameters().Length == 0);
            Debug.Assert(getMethod.ReturnType == typeof(TProperty));

            PropertyGetterDelegate getter =
                (PropertyGetterDelegate)getMethod.CreateDelegate(typeof(PropertyGetterDelegate));

            return new ClassPropertyGetter<TDeclaring, TProperty>(getter);
        }
    }
}
