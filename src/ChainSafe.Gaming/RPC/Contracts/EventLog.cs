using Nethereum.RPC.Eth.DTOs;

namespace ChainSafe.Gaming.Evm.Contracts
{
    /// <summary>
    /// Class that defines properties of an Event log.
    /// </summary>
    /// <typeparam name="T">Event type.</typeparam>
    public class EventLog<T> : IEventLog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventLog{T}"/> class.
        /// </summary>
        /// <param name="eventObject">The event object of type defined by type parameter T.</param>
        /// <param name="log">The log of type FilterLog.</param>
        public EventLog(T eventObject, FilterLog log)
        {
            Event = eventObject;
            Log = log;
        }

        /// <summary>
        /// Gets the event of type defined by type parameter T.
        /// </summary>
        /// <returns>Returns the event object.</returns>
        public T Event { get; }

        /// <summary>
        /// Gets the log of type FilterLog.
        /// </summary>
        /// <returns>Returns the log object.</returns>
        public FilterLog Log { get; }
    }
}