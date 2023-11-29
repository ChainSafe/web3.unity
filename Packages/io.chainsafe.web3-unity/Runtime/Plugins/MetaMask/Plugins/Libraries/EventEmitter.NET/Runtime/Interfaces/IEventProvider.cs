namespace EventEmitter.NET.Interfaces
{
    /// <summary>
    /// An interface that represents a class that can trigger event listeners for a given data type. This
    /// provider can trigger any event for a given event data type.
    /// </summary>
    /// <typeparam name="T">The event data type this class triggers events with</typeparam>
    public interface IEventProvider<in T>
    {
        /// <summary>
        /// Trigger the given event (using the event id) with the given event data.
        /// </summary>
        /// <param name="eventId">The event id to trigger</param>
        /// <param name="eventData">The event data to trigger the event with</param>
        void PropagateEvent(string eventId, T eventData);
    }
}