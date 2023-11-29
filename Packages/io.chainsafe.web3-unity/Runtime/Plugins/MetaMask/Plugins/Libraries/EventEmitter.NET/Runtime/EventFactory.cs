using System;
using System.Collections.Generic;
using EventEmitter.NET.Interfaces;

namespace EventEmitter.NET
{
    /// <summary>
    /// A class that simply holds the IEventProvider for a given event data type T. This is needed to keep the
    /// different event listeners (same eventId but different event data types) separate at runtime.
    ///
    /// Event Factories are seperated by context, and a context string must be provided before
    /// getting access to an EventFactory<T>. This means events ARE NOT fired between contexts
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventFactory<T>
    {
        private static Dictionary<string, EventFactory<T>> _eventFactories = new Dictionary<string, EventFactory<T>>();
        private static readonly object _factoryLock = new object();
        private IEventProvider<T> _eventProvider;
        
        /// <summary>
        /// The current context of this EventFactory
        /// </summary>
        public string Context { get; private set; }

        /// <summary>
        /// Create a new event factory with the given context string
        /// </summary>
        /// <param name="context">The context string to create this factory with</param>
        private EventFactory(string context)
        {
            this.Context = context;
        }

        /// <summary>
        /// Get the EventFactory for the event data type T
        /// <param name="context">The context string to use</param>
        /// <returns>The EventFactory that is isolated in the given context string</returns>
        /// </summary>
        public static EventFactory<T> InstanceOf(string context)
        {
            lock (_factoryLock)
            {
                if (!_eventFactories.ContainsKey(context))
                    _eventFactories.Add(context, new EventFactory<T>(context));

                return _eventFactories[context];
            }
        }
        
        /// <summary>
        /// Get the current EventProvider for the event data type T
        /// </summary>
        /// <exception cref="Exception">Internally only. When this value is set more than once</exception>
        public IEventProvider<T> Provider
        {
            get
            {
                return _eventProvider;
            }
            internal set
            {            
                if (_eventProvider != null)
                    throw new Exception("Provider for type " + typeof(T) + " already set");
                
                _eventProvider = value;
            }
        }
    }
}
