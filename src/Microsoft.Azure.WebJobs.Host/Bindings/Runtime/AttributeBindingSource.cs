// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.WebJobs.Host.Bindings.Runtime
{
    internal class AttributeBindingSource : IAttributeBindingSource
    {
        private readonly MemberInfo _memberInfo;
        private readonly IBindingProvider _bindingProvider;
        private readonly AmbientBindingContext _context;

        public AttributeBindingSource(MemberInfo memberInfo, IBindingProvider bindingProvider, AmbientBindingContext context)
        {
            if (memberInfo == null)
            {
                throw new ArgumentNullException("memberInfo");
            }

            if (bindingProvider == null)
            {
                throw new ArgumentNullException("bindingProvider");
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _memberInfo = memberInfo;
            _bindingProvider = bindingProvider;
            _context = context;
        }

        public AmbientBindingContext AmbientBindingContext
        {
            get { return _context; }
        }

        public Task<IBinding> BindAsync<TValue>(Attribute attribute, Attribute[] additionalAttributes = null, CancellationToken cancellationToken = default(CancellationToken))
        {
#if !NETSTANDARD1_5
            ParameterInfo parameterInfo = new FakeParameterInfo(typeof(TValue), _memberInfo, attribute, additionalAttributes);
            BindingProviderContext bindingProviderContext =
                new BindingProviderContext(parameterInfo, bindingDataContract: null, cancellationToken: cancellationToken);

            return _bindingProvider.TryCreateAsync(bindingProviderContext);
#else
            // TODO: FACAVAL - We'll like need to refactor the way the SDK works with parameter info to eliminate the direct dependency
            return Task.FromResult<IBinding>(null);
#endif

        }
    }

#if !NETSTANDARD1_5
    // A non-reflection based implementation
    public class FakeParameterInfo : ParameterInfo, ICustomAttributeProvider
    {
        private readonly Collection<Attribute> _attributes = new Collection<Attribute>();

        public FakeParameterInfo(Type parameterType, MemberInfo memberInfo, Attribute attribute, Attribute[] additionalAttributes)
        {
            ParameterType = parameterType;
            Name = "?";
            Member = memberInfo;

            // union all the parameter attributes
            _attributes.Add(attribute);
            if (additionalAttributes != null)
            {
                foreach (var additionalAttribute in additionalAttributes)
                {
                    _attributes.Add(additionalAttribute);
                }
            }
        }

        public override Type ParameterType { get; }

        public override string Name { get; }

        public override MemberInfo Member { get; }

        public override ParameterAttributes Attributes => ParameterAttributes.In;

        object[] ICustomAttributeProvider.GetCustomAttributes(Type attributeType, bool inherit)
        {
            return _attributes.Where(p => p.GetType() == attributeType).ToArray();
        }
    }
#endif
}
