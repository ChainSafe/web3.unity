using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace ChainSafe.Gaming.Evm.Contracts.Builders.FilterInput
{
    /// <summary>
    /// A class that provides extension methods for Filters.
    /// </summary>
    public static class FilterExtensions
    {
        /// <summary>
        /// Returns a key which can be used to identify the FilterLog.
        /// </summary>
        /// <param name="log">The filter log instance.</param>
        /// <returns>The key as a string.</returns>
        public static string Key(this FilterLog log)
        {
            if (log.TransactionHash == null || log.LogIndex == null)
            {
                return log.GetHashCode().ToString();
            }

            return $"{log.TransactionHash}{log.LogIndex.HexValue}";
        }

        /// <summary>
        /// Merges a list of FilterLogs into a master dictionary.
        /// </summary>
        /// <param name="masterList">The master dictionary of FilterLogs.</param>
        /// <param name="candidates">Array of candidate FilterLogs to be added to the master dictionary.</param>
        /// <returns>The updated master dictionary.</returns>
        public static Dictionary<string, FilterLog> Merge(this Dictionary<string, FilterLog> masterList, FilterLog[] candidates)
        {
            foreach (var log in candidates)
            {
                var key = log.Key();

                if (!masterList.ContainsKey(key))
                {
                    masterList.Add(key, log);
                }
            }

            return masterList;
        }

        /// <summary>
        /// Returns the number of blocks in the Filterlog.
        /// </summary>
        /// <param name="filter">The NewFilterInput instance.</param>
        /// <returns>The number of blocks as BigInteger.</returns>
        public static BigInteger? NumberOfBlocksInBlockParameters(this NewFilterInput filter)
        {
            if (filter.FromBlock?.BlockNumber == null || filter.ToBlock?.BlockNumber == null)
            {
                return null;
            }

            return (filter.ToBlock.BlockNumber.Value - filter.FromBlock.BlockNumber.Value) + 1;
        }

        /// <summary>
        /// Sets the block range.
        /// </summary>
        /// <param name="filter">The NewFilterInput instance.</param>
        /// <param name="range">The BlockRange instance.</param>
        public static void SetBlockRange(this NewFilterInput filter, BlockRange range) =>
            SetBlockRange(filter, range.From, range.To);

        /// <summary>
        /// Sets the block range from a pair of BigIntegers.
        /// </summary>
        /// <param name="filter">The NewFilterInput instance.</param>
        /// <param name="from">The from block bound.</param>
        /// <param name="to">The to block bound.</param>
        public static void SetBlockRange(this NewFilterInput filter, BigInteger from, BigInteger to) =>
            SetBlockRange(filter, from.ToHexBigInteger(), to.ToHexBigInteger());

        /// <summary>
        /// Sets the block range from a pair of HexBigIntegers.
        /// </summary>
        /// <param name="filter">The NewFilterInput instance.</param>
        /// <param name="from">The from block bound as HexBigInteger.</param>
        /// <param name="to">The to block bound as HexBigInteger.</param>
        public static void SetBlockRange(this NewFilterInput filter, HexBigInteger from, HexBigInteger to)
        {
            filter.FromBlock = new BlockParameter(from);
            filter.ToBlock = new BlockParameter(to);
        }

        /// <summary>
        /// Returns if the Filter is Topic filtered or not.
        /// </summary>
        /// <param name="filter">The NewFilterInput instance.</param>
        /// <param name="topicNumber">The topicNumber to check.</param>
        /// <returns>True if the TopicNumber exists, otherwise False.</returns>
        public static bool IsTopicFiltered(this NewFilterInput filter, uint topicNumber)
        {
            var filterValue = filter.GetFirstTopicValue(topicNumber);
            return filterValue != null;
        }

        /// <summary>
        /// Returns first filter value as string.
        /// </summary>
        /// <param name="filter">The NewFilterInput instance.</param>
        /// <param name="topicNumber">The topicNumber to check.</param>
        /// <returns>The first filter value as string. Null if it doesn't exist.</returns>
        public static string GetFirstTopicValueAsString(this NewFilterInput filter, uint topicNumber)
        {
            var filterValue = filter.GetFirstTopicValue(topicNumber);
            return filterValue?.ToString();
        }

        /// <summary>
        /// Returns first filter value.
        /// </summary>
        /// <param name="filter">The NewFilterInput instance.</param>
        /// <param name="topicNumber">The topicNumber to check.</param>
        /// <returns>The first filter value as object. Null if it doesn't exist.</returns>
        public static object GetFirstTopicValue(this NewFilterInput filter, uint topicNumber)
        {
            var topicValues = filter.GetTopicValues(topicNumber);
            return topicValues.FirstOrDefault();
        }

        /// <summary>
        /// Returns filter values as an array for the provided topicNumber.
        /// </summary>
        /// <param name="filter">The NewFilterInput instance.</param>
        /// <param name="topicNumber">The topicNumber for which values are to be fetched.</param>
        /// <returns>An array of objects representing the filter values.</returns>
        public static object[] GetTopicValues(this NewFilterInput filter, uint topicNumber)
        {
            var allTopics = filter.Topics;

            if (allTopics == null)
            {
                return Array.Empty<object>();
            }

            if (allTopics.Length < 2)
            {
                return Array.Empty<object>();
            }

            if (topicNumber > allTopics.Length)
            {
                return Array.Empty<object>();
            }

            if (allTopics[topicNumber] is object[] topicValues)
            {
                return topicValues;
            }

            if (allTopics[topicNumber] is object val)
            {
                return new[] { val };
            }

            return Array.Empty<object>();
        }
    }
}
