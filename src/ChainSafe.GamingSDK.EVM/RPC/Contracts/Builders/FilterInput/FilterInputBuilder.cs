using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Nethereum.ABI.Model;
using Nethereum.RPC.Eth.DTOs;
using Web3Unity.Scripts.Library.Ethers.Contracts.Extensions;

namespace Web3Unity.Scripts.Library.Ethers.Contracts.Builders.FilterInput
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

        public FilterInputBuilder()
        {
            eventAbi = ABITypedRegistry.GetEvent<TEventDto>();
            topics = new TopicFilterContainer<TEventDto>();
        }

        public FilterInputBuilder<TEventDto> AddTopic<TPropertyType>(
            Expression<Func<TEventDto, TPropertyType>> propertySelector, IEnumerable<TPropertyType> desiredValues)
        {
            foreach (var desiredValue in desiredValues)
            {
                AddTopic(propertySelector, desiredValue);
            }

            return this;
        }

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

        public NewFilterInput Build(string contractAddress, BlockRange? blockRange = null)
        {
            return Build(new[] { contractAddress }, blockRange);
        }

        public NewFilterInput Build(string contractAddress, BlockParameter from, BlockParameter to)
        {
            return Build(new[] { contractAddress }, from, to);
        }

        public NewFilterInput Build(string[] contractAddresses = null, BlockRange? blockRange = null)
        {
            BlockParameter from = blockRange == null ? null : new BlockParameter(blockRange.Value.From);
            BlockParameter to = blockRange == null ? null : new BlockParameter(blockRange.Value.To);

            return Build(contractAddresses, from, to);
        }

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