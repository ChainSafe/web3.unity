using System;
using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.Model;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json.Linq;
using Web3Unity.Scripts.Library.Ethers.Contracts.Extensions;

namespace Web3Unity.Scripts.Library.Ethers.Contracts.Builders
{
    [Obsolete("Use the EventABI extensions instead")]
    public class EventBuilder
    {
        public EventBuilder(string contractAddress, EventABI eventAbi)
        {
            ContractAddress = contractAddress;
            EventABI = eventAbi;
        }

        public string ContractAddress { get; set; }

        public EventABI EventABI { get; }

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

        public List<EventLog<T>> DecodeAllEventsForEvent<T>(FilterLog[] logs)
            where T : new()
        {
            return EventABI.DecodeAllEvents<T>(logs);
        }

        public List<EventLog<T>> DecodeAllEventsForEvent<T>(JArray logs)
            where T : new()
        {
            return EventABI.DecodeAllEvents<T>(logs);
        }
    }
}