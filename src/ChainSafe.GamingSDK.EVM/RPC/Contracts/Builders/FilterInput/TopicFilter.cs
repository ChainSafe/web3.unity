using System.Collections.Generic;
using System.Reflection;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Web3Unity.Scripts.Library.Ethers.Contracts.Builders.FilterInput
{
    internal class TopicFilter
    {
        internal static readonly TopicFilter Empty = new(null, null);

        private List<object> values;

        internal TopicFilter(PropertyInfo eventDtoProperty, ParameterAttribute parameterAttribute)
        {
            EventDtoProperty = eventDtoProperty;
            ParameterAttribute = parameterAttribute;
        }

        public PropertyInfo EventDtoProperty { get; }

        public ParameterAttribute ParameterAttribute { get; }

        public object[] GetValues()
        {
            return values == null || values.Count == 0 ? null : values.ToArray();
        }

        public void AddValue(object val)
        {
            values ??= new List<object>();

            values.Add(val);
        }
    }
}