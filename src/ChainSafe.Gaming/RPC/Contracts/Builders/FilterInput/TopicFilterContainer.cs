using System;
using System.Linq;
using System.Reflection;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace ChainSafe.Gaming.Evm.Contracts.Builders.FilterInput
{
    /// <summary>
    /// A container for handling Topic filters.
    /// </summary>
    /// <typeparam name="T">A class type.</typeparam>
    internal class TopicFilterContainer<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TopicFilterContainer{T}"/> class.
        /// </summary>
        internal TopicFilterContainer()
        {
            var indexedParameters = PropertiesExtractor
                .GetPropertiesWithParameterAttribute(typeof(T))
                .Select(p => new TopicFilter(p, p.GetCustomAttribute<ParameterAttribute>(true)))
                .Where(p => p.ParameterAttribute?.Parameter.Indexed ?? false)
                .OrderBy(p => p.ParameterAttribute.Order)
                .ToArray();

            Empty = indexedParameters.Length == 0;

            Topic1 = indexedParameters.Length > 0 ? indexedParameters[0] : TopicFilter.Empty;
            Topic2 = indexedParameters.Length > 1 ? indexedParameters[1] : TopicFilter.Empty;
            Topic3 = indexedParameters.Length > 2 ? indexedParameters[2] : TopicFilter.Empty;

            Topics = new[] { Topic1, Topic2, Topic3 };
        }

        /// <summary>
        /// Gets a boolean value indicating if TopicFilterContainer is empty.
        /// </summary>
        public bool Empty { get; private set; }

        /// <summary>
        /// Gets the first TopicFilter object.
        /// </summary>
        public TopicFilter Topic1 { get; private set; }

        /// <summary>
        /// Gets the second TopicFilter object.
        /// </summary>
        public TopicFilter Topic2 { get; private set; }

        /// <summary>
        /// Gets the third TopicFilter object.
        /// </summary>
        public TopicFilter Topic3 { get; private set; }

        /// <summary>
        /// Private set for Topics array.
        /// </summary>
        private TopicFilter[] Topics { get; set; }

        /// <summary>
        /// Returns the TopicFilter object associated with the given PropertyInfo.
        /// </summary>
        /// <param name="pInfo">The PropertyInfo object to match with a TopicFilter object.</param>
        /// <returns>The TopicFilter object that matches the provided PropertyInfo.</returns>
        /// <exception cref="ArgumentException">Throws an exception if a matching TopicFilter can't be found.</exception>
        public TopicFilter GetTopic(PropertyInfo pInfo)
        {
            return
                Topics.FirstOrDefault(t => t.EventDtoProperty.Name == pInfo.Name) ??
                throw new ArgumentException($"Property '{pInfo.Name}' does not represent a topic. The property must have a ParameterAttribute which is flagged as indexed");
        }
    }
}