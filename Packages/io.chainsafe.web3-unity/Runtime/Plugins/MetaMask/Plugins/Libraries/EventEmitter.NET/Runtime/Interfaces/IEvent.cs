namespace EventEmitter.NET.Interfaces
{
    /// <summary>
    /// An interface that represents a class that can take in an event's data and store it. The event data
    /// is generic and may be anything
    /// </summary>
    /// <typeparam name="T">The type of the event data</typeparam>
    public interface IEvent<in T>
    {
        /// <summary>
        /// Set the data for this event. This is invoked when the event is triggered with the provided
        /// data
        /// </summary>
        /// <param name="data">The data the event was triggered with</param>
        void SetData(T data);
    }
}