using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using ChainSafe.Gaming.Evm.Contracts.Extensions;
using Nethereum.ABI.Model;
using Nethereum.RPC.Eth.DTOs;

namespace ChainSafe.Gaming.Evm.Contracts.Builders.FilterInput
{
    /// <summary>
    /// Builds a filter based on indexed parameters on an event DTO query template.
    /// The DTO should have properties decorated with ParameterAttribute
    /// Only ParameterAttributes flagged as indexed are included
    /// Use SetTopic to set a value on a indexed property on the query template
    /// Values set on the query template are put in to the filter when Build is called.
    /// </summary>
    /// <typeparam name="TEventDto">The event DTO type.</typeparam>
    public class FilterInputBuilder<TEventDto>
        where TEventDto : class
    {
        private readonly EventABI eventAbi;
        private readonly TopicFilterContainer<TEventDto> topics;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterInputBuilder{TEventDto}"/> class with the given event DTO type.
        /// </summary>
        public FilterInputBuilder()
        {
            eventAbi = ABITypedRegistry.GetEvent<TEventDto>();
            topics = new TopicFilterContainer<TEventDto>();
        }

        /// <summary>
        /// Adds topics to the filter.
        /// </summary>
        /// <param name="propertySelector">The property to select.</param>
        /// <param name="desiredValues">The values of the property to select.</param>
        /// <returns>A FilterInputBuilder of the modified filter.</returns>
        /// <typeparam name="TPropertyType">Generic type parameter of the property.</typeparam>
        public FilterInputBuilder<TEventDto> AddTopic<TPropertyType>(
            Expression<Func<TEventDto, TPropertyType>> propertySelector, IEnumerable<TPropertyType> desiredValues)
        {
            foreach (var desiredValue in desiredValues)
            {
                AddTopic(propertySelector, desiredValue);
            }

            return this;
        }

        /// <summary>
        /// Adds a topic to the filter.
        /// </summary>
        /// <param name="propertySelector">The property to select.</param>
        /// <param name="desiredValue">The value of the property to select.</param>
        /// <returns>A FilterInputBuilder of the modified filter.</returns>
        /// <typeparam name="TPropertyType">Generic type parameter of the property.</typeparam>
        public FilterInputBuilder<TEventDto> AddTopic<TPropertyType>(
            Expression<Func<TEventDto, TPropertyType>> propertySelector, TPropertyType desiredValue)
        {
            var member = propertySelector.Body as MemberExpression;
            var propertyInfo = member?.Member as PropertyInfo;

            topics
                .GetTopic(propertyInfo)
                .AddValue(desiredValue);

            return this;
        }

        /// <summary>
        /// Builds a filter based on a contract address and block range.
        /// </summary>
        /// <param name="contractAddress">The contract address.</param>
        /// <param name="blockRange">The block range.</param>
        /// <returns>A NewFitlerInput with the specified filter settings.</returns>
        public NewFilterInput Build(string contractAddress, BlockRange? blockRange = null)
        {
            return Build(new[] { contractAddress }, blockRange);
        }

        /// <summary>
        /// Builds a filter based on a contract address and block parameter values.
        /// </summary>
        /// <param name="contractAddress">The contract address.</param>
        /// <param name="from">The 'from' block parameter.</param>
        /// <param name="to">The 'to' block parameter.</param>
        /// <returns>A NewFilterInput with the specified filter settings.</returns>
        public NewFilterInput Build(string contractAddress, BlockParameter from, BlockParameter to)
        {
            return Build(new[] { contractAddress }, from, to);
        }

        /// <summary>
        /// Builds a filter based on multiple contract addresses and/or a block range.
        /// </summary>
        /// <param name="contractAddresses">The array of contract addresses.</param>
        /// <param name="blockRange">The block range.</param>
        /// <returns>A NewFilterInput with the specified filter settings.</returns>
        public NewFilterInput Build(string[] contractAddresses = null, BlockRange? blockRange = null)
        {
            BlockParameter from = blockRange == null ? null : new BlockParameter(blockRange.Value.From);
            BlockParameter to = blockRange == null ? null : new BlockParameter(blockRange.Value.To);

            return Build(contractAddresses, from, to);
        }

        /// <summary>
        /// Builds a filter based on contract addresses and block parameter values.
        /// </summary>
        /// <param name="contractAddresses">The array of contract addresses.</param>
        /// <param name="from">The 'from' block parameter.</param>
        /// <param name="to">The 'to' block parameter.</param>
        /// <returns>A NewFilterInput with the specified filter settings.</returns>
        public NewFilterInput Build(string[] contractAddresses, BlockParameter from, BlockParameter to)
        {
            if (topics.Empty)
            {
                return eventAbi.CreateFilterInput(contractAddresses, from, to);
            }

            // if the object array exceeds the length of the topics on the abi
            // the filter no longer works

            // one indexed topic
            if (topics.Topic2 == TopicFilter.Empty)
            {
                return eventAbi.CreateFilterInput(
                    contractAddresses,
                    topics.Topic1.GetValues(),
                    from,
                    to);
            }

            // two indexed topics
            if (topics.Topic3 == TopicFilter.Empty)
            {
                return eventAbi.CreateFilterInput(
                    contractAddresses,
                    topics.Topic1.GetValues(),
                    topics.Topic2.GetValues(),
                    from,
                    to);
            }

            // three indexed topics
            return eventAbi.CreateFilterInput(
                contractAddresses,
                topics.Topic1.GetValues(),
                topics.Topic2.GetValues(),
                topics.Topic3.GetValues(),
                from,
                to);
        }
    }
}