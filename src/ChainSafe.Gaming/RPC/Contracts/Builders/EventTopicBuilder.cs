using System;
using System.Linq;
using Nethereum.ABI.Model;
using Nethereum.Hex.HexConvertors.Extensions;

namespace ChainSafe.Gaming.Evm.Contracts.Builders
{
    /// <summary>
    /// Event topic builder. Using this class you can build the topics for a given event.
    /// </summary>
    public class EventTopicBuilder
    {
        private readonly EventABI eventABI;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTopicBuilder"/> class.
        /// Constructor for the EventTopicBuilder class.
        /// </summary>
        /// <param name="eventABI">ABI of the event.</param>
        public EventTopicBuilder(EventABI eventABI)
        {
            this.eventABI = eventABI;
        }

        private static string EnsureHexPrefix(string input)
        {
            return input.EnsureHexPrefix();
        }

        /// <summary>
        /// Returns the event signature topic as the only topic.
        /// </summary>
        /// <returns>Object array with the event signature topic.</returns>
        public object[] GetSignatureTopicAsTheOnlyTopic()
        {
            return new object[] { GetSignatureTopic() };
        }

        /// <summary>
        /// Returns the event signature topic.
        /// </summary>
        /// <returns>Object that represents the event signature topic.</returns>
        public object GetSignatureTopic()
        {
            return eventABI.Sha3Signature.EnsureHexPrefix();
        }

        /// <summary>
        /// Gets the event topics using first the provided topic.
        /// </summary>
        /// <returns>Object array representing the event topics.</returns>
        public object[] GetTopics(object[] firstTopic)
        {
            if (eventABI.IsAnonymous)
            {
                return new[] { GetValueTopic(firstTopic, 1) };
            }

            return new[] { GetSignatureTopic(), GetValueTopic(firstTopic, 1) };
        }

        /// <summary>
        /// Gets the event topics using the provided first and second topics.
        /// </summary>
        /// <returns>Object array representing the event topics.</returns>
        public object[] GetTopics(object[] firstTopic, object[] secondTopic)
        {
            if (eventABI.IsAnonymous)
            {
                return new[] { GetValueTopic(firstTopic, 1), GetValueTopic(secondTopic, 2) };
            }

            return new[] { GetSignatureTopic(), GetValueTopic(firstTopic, 1), GetValueTopic(secondTopic, 2) };
        }

        /// <summary>
        /// Gets the event topics using the provided first, second, and third topics.
        /// </summary>
        /// <returns>Object array representing the event topics.</returns>
        public object[] GetTopics(object[] firstTopic, object[] secondTopic, object[] thirdTopic)
        {
            if (eventABI.IsAnonymous)
            {
                return new[]
                {
                    GetValueTopic(firstTopic, 1), GetValueTopic(secondTopic, 2),
                    GetValueTopic(thirdTopic, 3),
                };
            }

            return new[]
            {
                GetSignatureTopic(), GetValueTopic(firstTopic, 1), GetValueTopic(secondTopic, 2),
                GetValueTopic(thirdTopic, 3),
            };
        }

        /// <summary>
        /// Gets the event topics using the provided first topics.
        /// </summary>
        /// <param name="firstTopic">First topic.</param>
        /// <returns>Object array representing the event topics.</returns>
        public object[] GetTopics(object firstTopic)
        {
            return GetTopics(new[] { firstTopic });
        }

        /// <summary>
        /// Gets the event topics using the provided first and second topics.
        /// </summary>
        /// <param name="firstTopic">First topic.</param>
        /// <param name="secondTopic">Second topic.</param>
        /// <returns>Object array representing the event topics.</returns>
        public object[] GetTopics(object firstTopic, object secondTopic)
        {
            return GetTopics(new[] { firstTopic }, new[] { secondTopic });
        }

        /// <summary>
        /// Gets the event topics using the provided first, second, and third topics.
        /// </summary>
        /// <param name="firstTopic">First topic.</param>
        /// <param name="secondTopic">Second topic.</param>
        /// <param name="thirdTopic">Third topic.</param>
        /// <returns>Object array representing the event topics.</returns>
        public object[] GetTopics(object firstTopic, object secondTopic, object thirdTopic)
        {
            return GetTopics(new[] { firstTopic }, new[] { secondTopic }, new[] { thirdTopic });
        }

        /// <summary>
        /// Returns topics for the given first using generic types.
        /// </summary>
        /// <param name="firstTopic">First topic.</param>
        /// <typeparam name="T1">Generic type of the first topic.</typeparam>
        /// <returns>Object array representing the event topics.</returns>
        public object[] GetTopics<T1>(T1 firstTopic)
        {
            return GetTopics(firstTopic == null ? null : new[] { (object)firstTopic });
        }

        /// <summary>
        /// Returns topics for the given first and second topics using generic types.
        /// </summary>
        /// <param name="firstTopic">First topic.</param>
        /// <param name="secondTopic">Second topic.</param>
        /// <typeparam name="T1">Generic type of the first topic.</typeparam>
        /// <typeparam name="T2">Generic type of the second topic.</typeparam>
        /// <returns>Object array representing the event topics.</returns>
        public object[] GetTopics<T1, T2>(T1 firstTopic, T2 secondTopic)
        {
            return GetTopics(
                firstTopic == null ? null : new[] { (object)firstTopic },
                secondTopic == null ? null : new[] { (object)secondTopic });
        }

        /// <summary>
        /// Returns topics for the given first, second and third topics using generic types.
        /// </summary>
        /// <param name="firstTopic">First topic.</param>
        /// <param name="secondTopic">Second topic.</param>
        /// <param name="thirdTopic">third topic.</param>
        /// <typeparam name="T1">Generic type of the first topic.</typeparam>
        /// <typeparam name="T2">Generic type of the second topic.</typeparam>
        /// <typeparam name="T3">Generic type of the third topic.</typeparam>
        /// <returns>Object array representing the event topics.</returns>
        public object[] GetTopics<T1, T2, T3>(T1 firstTopic, T2 secondTopic, T3 thirdTopic)
        {
            return GetTopics(
                firstTopic == null ? null : new[] { (object)firstTopic },
                secondTopic == null ? null : new[] { (object)secondTopic },
                thirdTopic == null ? null : new[] { (object)thirdTopic });
        }

        /// <summary>
        /// Gets the event topics using the provided first topics.
        /// </summary>
        /// <param name="firstOrTopics">First topic.</param>
        /// <typeparam name="T1">Generic type of the first topic.</typeparam>
        /// <returns>Object array representing the event topics.</returns>
        public object[] GetTopics<T1>(T1[] firstOrTopics)
        {
            return GetTopics(firstOrTopics.Cast<object>().ToArray());
        }

        /// <summary>
        /// Gets the event topics using the provided first and second topics.
        /// </summary>
        /// <param name="firstOrTopics">First topic.</param>
        /// <param name="secondOrTopics">Second topic.</param>
        /// <typeparam name="T1">Generic type of the first topic.</typeparam>
        /// <typeparam name="T2">Generic type of the second topic.</typeparam>
        /// <returns>Object array representing the event topics.</returns>
        public object[] GetTopics<T1, T2>(T1[] firstOrTopics, T2[] secondOrTopics)
        {
            return GetTopics(firstOrTopics.Cast<object>().ToArray(), secondOrTopics.Cast<object>().ToArray());
        }

        /// <summary>
        /// Gets the event topics using the provided first, second, and third topics.
        /// </summary>
        /// <param name="firstOrTopics">First topic.</param>
        /// <param name="secondOrTopics">Second topic.</param>
        /// <param name="thirdOrTopics">Third topic.</param>
        /// <typeparam name="T1">Generic type of the first topic.</typeparam>
        /// <typeparam name="T2">Generic type of the second topic.</typeparam>
        /// <typeparam name="T3">Generic type of the third topic.</typeparam>
        /// <returns>Object array representing the event topics.</returns>
        public object[] GetTopics<T1, T2, T3>(T1[] firstOrTopics, T2[] secondOrTopics, T3[] thirdOrTopics)
        {
            return GetTopics(firstOrTopics.Cast<object>().ToArray(), secondOrTopics.Cast<object>().ToArray(), thirdOrTopics.Cast<object>().ToArray());
        }

        /// <summary>
        /// Gets the value topic for the given values and parameter number.
        /// </summary>
        /// <param name="values">Array of values.</param>
        /// <param name="paramNumber">Number of the parameter.</param>
        /// <returns>Array of objects representing the value topic.</returns>
        /// <exception cref="Exception">Exception thrown when the event parameter is not found.</exception>
        public object[] GetValueTopic(object[] values, int paramNumber)
        {
            if (values == null)
            {
                return null;
            }

            var encoded = new object[values.Length];
            var parameter = eventABI.InputParameters.FirstOrDefault(x => x.Order == paramNumber) ??
                throw new Exception("Event parameter not found at " + paramNumber);
            for (var i = 0; i < values.Length; i++)
            {
                if (values[i] != null)
                {
                    encoded[i] = EnsureHexPrefix(parameter.ABIType.Encode(values[i]).ToHex());
                }
            }

            return encoded;
        }
    }
}