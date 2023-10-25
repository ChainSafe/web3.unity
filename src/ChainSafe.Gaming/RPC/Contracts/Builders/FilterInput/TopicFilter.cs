using System.Collections.Generic;
using System.Reflection;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace ChainSafe.Gaming.Evm.Contracts.Builders.FilterInput
{
    /// <summary>
    /// Defines a TopicFilter class that is used to filter topic events.
    /// </summary>
    internal class TopicFilter
    {
        /// <summary>
        /// Represents an empty TopicFilter object.
        /// </summary>
        internal static readonly TopicFilter Empty = new(null, null);

        private List<object> values;

        /// <summary>
        /// Initializes a new instance of the <see cref="TopicFilter"/> class.
        /// </summary>
        /// <param name="eventDtoProperty">This property holds event data transfer object.</param>
        /// <param name="parameterAttribute">This holds the parameter attribute values.</param>
        internal TopicFilter(PropertyInfo eventDtoProperty, ParameterAttribute parameterAttribute)
        {
            EventDtoProperty = eventDtoProperty;
            ParameterAttribute = parameterAttribute;
        }

        /// <summary>
        /// Gets event data transfer object property.
        /// </summary>
        /// <returns>Returns the property info of event data transfer object.</returns>
        public PropertyInfo EventDtoProperty { get; }

        /// <summary>
        /// Gets parameter attribute.
        /// </summary>
        /// <returns>Returns the parameter attribute.</returns>
        public ParameterAttribute ParameterAttribute { get; }

        /// <summary>
        /// Returns an array of values.
        /// </summary>
        /// <returns>Returns an array of values if there are any, or null otherwise.</returns>
        public object[] GetValues()
        {
            return values == null || values.Count == 0 ? null : values.ToArray();
        }

        /// <summary>
        /// Adds a value to the values list.
        /// </summary>
        /// <param name="val">This object value to add to the list.</param>
        public void AddValue(object val)
        {
            values ??= new List<object>();

            values.Add(val);
        }
    }
}