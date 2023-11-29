using System;
using EventEmitter.NET.Interfaces;

namespace EventEmitter.NET.Model
{
    /// <summary>
    /// A generic implementation of the IEvent interface. Given a event data type T, store the data in-memory
    /// in the EventData property
    /// </summary>
    /// <typeparam name="T">The event data type to store</typeparam>
    public class GenericEvent<T> : IEvent<T>
    {
        /// <summary>
        /// The event data
        /// </summary>
        public T EventData { get; private set; }

        /// <summary>
        /// Store the event data, this function may only be invoked once
        /// </summary>
        /// <param name="response">The event data to store</param>
        /// <exception cref="ArgumentException">This instance already is storing event data</exception>
        public void SetData(T response)
        {
            if (EventData != null && !EventData.Equals(default(T)))
            {
                throw new ArgumentException("Event Data was already set");
            }
            
            EventData = response;
        }
    }
}
