using System;
using System.Collections.Generic;

namespace EventEmitter.NET
{
    /// <summary>
    /// A mapping of eventIds to EventHandler objects. This using a Dictionary as the backing datastore
    /// </summary>
    /// <typeparam name="TEventArgs">The type of EventHandler's argument to store</typeparam>
    public class EventHandlerMap<TEventArgs>
    {
        private Dictionary<string, EventHandler<TEventArgs>> mapping =
            new Dictionary<string, EventHandler<TEventArgs>>();

        private readonly object _mappingLock = new object();

        private EventHandler<TEventArgs> BeforeEventExecuted;

        /// <summary>
        /// Create a new EventHandlerMap with an initial EventHandler to append onto
        /// </summary>
        /// <param name="callbackBeforeExecuted">The initial EventHandler to use as the EventHandler.</param>
        public EventHandlerMap(EventHandler<TEventArgs> callbackBeforeExecuted)
        {
            if (callbackBeforeExecuted == null)
            {
                callbackBeforeExecuted = CallbackBeforeExecuted;
            }

            this.BeforeEventExecuted = callbackBeforeExecuted;
        }

        private void CallbackBeforeExecuted(object sender, TEventArgs e)
        {
        }

        /// <summary>
        /// Get an EventHandler by its eventId. If the provided eventId does not exist, then the
        /// initial EventHandler is returned and tracking begins
        /// </summary>
        /// <param name="eventId">The eventId of the EventHandler</param>
        public EventHandler<TEventArgs> this[string eventId]
        {
            get
            {
                lock (_mappingLock)
                {
                    if (!mapping.ContainsKey(eventId))
                    {
                        mapping.Add(eventId, BeforeEventExecuted);
                    }

                    return mapping[eventId];
                }
            }
            set
            {
                lock (_mappingLock)
                {
                    if (mapping.ContainsKey(eventId))
                    {
                        mapping.Remove(eventId);
                    }

                    mapping.Add(eventId, value);
                }
            }
        }

        /// <summary>
        /// Check if a given eventId has any EventHandlers registered yet.
        /// </summary>
        /// <param name="eventId">The eventId to check for</param>
        /// <returns>true if the eventId has any EventHandlers, false otherwise</returns>
        public bool Contains(string eventId)
        {
            lock (_mappingLock)
            {
                return mapping.ContainsKey(eventId);
            }
        }

        /// <summary>
        /// Clear an eventId from the mapping
        /// </summary>
        /// <param name="eventId">The eventId to clear</param>
        public void Clear(string eventId)
        {
            lock (_mappingLock)
            {
                if (mapping.ContainsKey(eventId))
                {
                    mapping.Remove(eventId);
                }
            }
        }
    }
}
