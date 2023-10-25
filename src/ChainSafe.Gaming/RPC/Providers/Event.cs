using System;
using System.Collections.Generic;
using System.Linq;

namespace ChainSafe.Gaming.Evm.Providers
{
    /// <summary>
    /// Abstract public class representing an event.
    /// </summary>
    public abstract class Event
    {
        private readonly List<string> pollableEvents = new()
        {
            "block",
            "network",
            "pending",
            "poll",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        /// <param name="tag">Event tag, a string.</param>
        /// <param name="once">Event execution constraint, a bool.</param>
        public Event(string tag, bool once)
        {
            Once = once;
            Tag = tag;
        }

        /// <summary>
        /// To check if event occurs once.
        /// </summary>
        public bool Once { get; set; }

        /// <summary>
        /// Event tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Event type derived from event tag.
        /// </summary>
        public string Type => Tag.Substring(0, Tag.IndexOf(":"));

        /// <summary>
        /// Event hash.
        /// </summary>
        public string Hash
        {
            get
            {
                var comps = new string(Tag.TakeWhile(c => c != ':').ToArray());
                return comps != "tx" ? null : comps;
            }
        }

        /// <summary>
        /// Probe if the event is pollable or not.
        /// </summary>
        public bool IsPollable => Tag.IndexOf(":", StringComparison.Ordinal) >= 0 || pollableEvents.IndexOf(Tag) >= 0;

        /*
        // public EventType Event()
        public string _Event()
        {
            return Type switch
            {
                "tx" => Hash,
                // "filter" => Filter,
                _ => Tag
            };
        }
        */

        /// <summary>
        /// Method to apply the event.
        /// </summary>
        /// <param name="args">Arguments to be used while applying/calling the event.</param>
        public abstract void Apply(object[] args);
    }

    /// <summary>
    /// Public class for single-parametric Event instruction.
    /// </summary>
    /// <typeparam name="T">Type that defines the event.</typeparam>
    public class Event<T> : Event
    {
        private readonly Func<T, object> listener;

        /// <summary>
        /// Initializes a new instance of the <see cref="Event{T}"/> class.
        /// Constructor for Event class with single parameter.
        /// </summary>
        /// <param name="tag">Event tag, a string.</param>
        /// <param name="listener">Function to be called.</param>
        /// <param name="once">Event execution constraint, a bool.</param>
        public Event(string tag, Func<T, object> listener, bool once)
            : base(tag, once)
        {
            this.listener = listener;
        }

        /// <summary>
        /// Method to apply the event.
        /// </summary>
        /// <param name="args">Arguments to be used while applying/calling the event.</param>
        public override void Apply(object[] args)
        {
            listener((T)args[0]);
        }
    }

    /// <summary>
    /// Public class for double-parametric Event instruction.
    /// </summary>
    /// <typeparam name="T1">First type that defines the event.</typeparam>
    /// <typeparam name="T2">Second type that defines the event.</typeparam>
    public class Event<T1, T2> : Event
    {
        private readonly Func<T1, T2, object> listener;

        /// <summary>
        /// Initializes a new instance of the <see cref="Event{T1, T2}"/> class.
        /// Constructor for Event class with two parameters.
        /// </summary>
        /// <param name="tag">Event tag, a string.</param>
        /// <param name="listener">Function to be called with two arguments.</param>
        /// <param name="once">Event execution constraint, a bool.</param>
        public Event(string tag, Func<T1, T2, object> listener, bool once)
            : base(tag, once)
        {
            this.listener = listener;
        }

        /// <summary>
        /// Method to apply the event.
        /// </summary>
        /// <param name="args">Arguments to be used while applying/calling the event.</param>
        public override void Apply(object[] args)
        {
            listener((T1)args[0], (T2)args[1]);
        }
    }
}