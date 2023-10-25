using System;
using System.Collections.Generic;
using ChainSafe.Gaming.Evm.Contracts.Extensions;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.Model;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json.Linq;

namespace ChainSafe.Gaming.Evm.Contracts.Builders
{
    /// <summary>
    /// Class that creates and manipulates events on an Ethereum contract.
    /// This class is obsolete and it's recommended to use the EventABI extensions instead.
    /// </summary>
    [Obsolete("Use the EventABI extensions instead")]
    public class EventBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventBuilder"/> class.
        /// </summary>
        /// <param name="contractAddress">The Ethereum contract address.</param>
        /// <param name="eventAbi">The event ABI of the Ethereum contract.</param>
        public EventBuilder(string contractAddress, EventABI eventAbi)
        {
            ContractAddress = contractAddress;
            EventABI = eventAbi;
        }

        /// <summary>
        /// Gets or sets the address of the Ethereum contract.
        /// </summary>
        public string ContractAddress { get; set; }

        /// <summary>
        /// Gets the EventABI instance related to the Ethereum contract.
        /// </summary>
        public EventABI EventABI { get; }

        /// <summary>
        /// Decodes all events for a given type.
        /// </summary>
        /// <typeparam name="T">The type of the event.</typeparam>
        /// <param name="logs">The logs for an event on an Ethereum contract.</param>
        /// <returns>A list of decoded events.</returns>
        public static List<EventLog<T>> DecodeAllEvents<T>(FilterLog[] logs)
            where T : new()
        {
            var result = new List<EventLog<T>>();
            if (logs == null)
            {
                return result;
            }

            var eventDecoder = new EventTopicDecoder();
            foreach (var log in logs)
            {
                var eventObject = eventDecoder.DecodeTopics<T>(log.Topics, log.Data);
                result.Add(new EventLog<T>(eventObject, log));
            }

            return result;
        }

        public NewFilterInput CreateFilterInput(BlockParameter fromBlock = null, BlockParameter toBlock = null)
        {
            return EventABI.CreateFilterInput(ContractAddress, fromBlock, toBlock);
        }

        public NewFilterInput CreateFilterInput(
            object[] filterTopic1,
            BlockParameter fromBlock = null,
            BlockParameter toBlock = null)
        {
            return EventABI.CreateFilterInput(ContractAddress, filterTopic1, fromBlock, toBlock);
        }

        public NewFilterInput CreateFilterInput(
            object[] filterTopic1,
            object[] filterTopic2,
            BlockParameter fromBlock = null,
            BlockParameter toBlock = null)
        {
            return EventABI.CreateFilterInput(ContractAddress, filterTopic1, filterTopic2, fromBlock, toBlock);
        }

        public NewFilterInput CreateFilterInput(
            object[] filterTopic1,
            object[] filterTopic2,
            object[] filterTopic3,
            BlockParameter fromBlock = null,
            BlockParameter toBlock = null)
        {
            return EventABI.CreateFilterInput(ContractAddress, filterTopic1, filterTopic2, filterTopic3, fromBlock, toBlock);
        }

        /// <summary>
        /// Checks if the log is related to the specific event.
        /// </summary>
        /// <param name="log">The log to check.</param>
        /// <returns><c>true</c> if the log is for the event, <c>false</c> otherwise.</returns>
        public bool IsLogForEvent(JToken log)
        {
            return EventABI.IsLogForEvent(log);
        }

        public bool IsLogForEvent(FilterLog log)
        {
            return EventABI.IsLogForEvent(log);
        }

        public bool IsFilterInputForEvent(NewFilterInput filterInput)
        {
            return EventABI.IsFilterInputForEvent(ContractAddress, filterInput);
        }

        public FilterLog[] GetLogsForEvent(JArray logs)
        {
            return EventABI.GetLogsForEvent(logs);
        }

        /// <summary>
        /// Decodes all events for a specific event.
        /// </summary>
        /// <typeparam name="T">The type of the event.</typeparam>
        /// <param name="logs">The logs for an event on an Ethereum contract.</param>
        /// <returns>A list of decoded events.</returns>
        public List<EventLog<T>> DecodeAllEventsForEvent<T>(FilterLog[] logs)
            where T : new()
        {
            return EventABI.DecodeAllEvents<T>(logs);
        }

        /// <summary>
        /// Decodes all events for a specific event.
        /// </summary>
        /// <typeparam name="T">The type of the event.</typeparam>
        /// <param name="logs">The logs (in JArray format) for an event on an Ethereum contract.</param>
        /// <returns>A list of decoded events.</returns>
        public List<EventLog<T>> DecodeAllEventsForEvent<T>(JArray logs)
            where T : new()
        {
            return EventABI.DecodeAllEvents<T>(logs);
        }
    }
}
