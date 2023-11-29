using System;
using EventEmitter.NET.Interfaces;
using EventEmitter.NET.Model;

namespace EventEmitter.NET
{
    /// <summary>
    /// Extension methods for any class that implements IEvents.
    /// </summary>
    public static class EventsExtensions
    {
        /// <summary>
        /// Listen for an eventId and invoke the given Action. This will trigger for
        /// any event data type.
        /// </summary>
        /// <param name="eventEmitter">The event emitter to use</param>
        /// <param name="eventId">The event id to listen to</param>
        /// <param name="callback">The callback Action to invoke when the event id (of any type) is triggered</param>
        public static IEvents On(this IEvents eventEmitter, string eventId, Action callback)
        {
            eventEmitter.On<object>(eventId, (sender, @event) =>
            {
                callback();
            });

            return eventEmitter;
        }

        /// <summary>
        /// Listen for an eventId and invoke the given Action. This will trigger for
        /// the given event data type T only.
        /// </summary>
        /// <param name="eventEmitter">The event emitter to use</param>
        /// <param name="eventId">The event id to listen to</param>
        /// <param name="callback">The callback Action to invoke when the event id is triggered</param>
        /// <typeparam name="T">The type of event data to listen for</typeparam>
        public static IEvents On<T>(this IEvents eventEmitter, string eventId, EventHandler<GenericEvent<T>> callback)
        {
            eventEmitter.Events.ListenFor(eventId, callback);

            return eventEmitter;
        }

        /// <summary>
        /// Listen for an eventId and invoke the given Action once. This will trigger for
        /// the given event data type T only.
        /// </summary>
        /// <param name="eventEmitter">The event emitter to use</param>
        /// <param name="eventId">The event id to listen to</param>
        /// <param name="callback">The callback Action to invoke when the event id is triggered</param>
        /// <typeparam name="T">The type of event data to listen for</typeparam>
        public static IEvents Once<T>(this IEvents eventEmitter, string eventId, EventHandler<GenericEvent<T>> callback)
        {
            eventEmitter.Events.ListenForOnce(eventId, callback);

            return eventEmitter;
        }
        
        /// <summary>
        /// Stop listening for an eventId with the given Action.
        /// </summary>
        /// <param name="eventEmitter">The event emitter to use</param>
        /// <param name="eventId">The event id to stop listening to</param>
        /// <param name="callback">The callback Action to stop invoking when the event id is triggered</param>
        /// <typeparam name="T">The type of event data to stop listening for</typeparam>
        public static IEvents Off<T>(this IEvents eventEmitter, string eventId, EventHandler<GenericEvent<T>> callback)
        {
            eventEmitter.Events.RemoveListener(eventId, callback);

            return eventEmitter;
        }

        /// <summary>
        /// Stop listening for an eventId with the given Action.
        /// </summary>
        /// <param name="eventEmitter">The event emitter to use</param>
        /// <param name="eventId">The event id to stop listening to</param>
        /// <param name="callback">The callback Action to stop invoking when the event id is triggered</param>
        /// <typeparam name="T">The type of event data to stop listening for</typeparam>
        public static IEvents RemoveListener<T>(this IEvents eventEmitter, string eventId,
            EventHandler<GenericEvent<T>> callback)
        {
            eventEmitter.Events.RemoveListener(eventId, callback);

            return eventEmitter;
        }
    }
}
