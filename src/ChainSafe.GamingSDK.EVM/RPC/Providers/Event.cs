using System;
using System.Collections.Generic;
using System.Linq;

namespace Web3Unity.Scripts.Library.Ethers.Providers
{
    public abstract class Event
    {
        private readonly List<string> pollableEvents = new()
        {
            "block",
            "network",
            "pending",
            "poll",
        };

        public Event(string tag, bool once)
        {
            Once = once;
            Tag = tag;
        }

        public bool Once { get; set; }

        public string Tag { get; set; }

        public string Type => Tag.Substring(0, Tag.IndexOf(":"));

        public string Hash
        {
            get
            {
                var comps = new string(Tag.TakeWhile(c => c != ':').ToArray());
                return comps != "tx" ? null : comps;
            }
        }

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

        public abstract void Apply(object[] args);
    }

    public class Event<T> : Event
    {
        private readonly Func<T, object> listener;

        public Event(string tag, Func<T, object> listener, bool once)
            : base(tag, once)
        {
            this.listener = listener;
        }

        public override void Apply(object[] args)
        {
            listener((T)args[0]);
        }
    }

    public class Event<T1, T2> : Event
    {
        private readonly Func<T1, T2, object> listener;

        public Event(string tag, Func<T1, T2, object> listener, bool once)
            : base(tag, once)
        {
            this.listener = listener;
        }

        public override void Apply(object[] args)
        {
            listener((T1)args[0], (T2)args[1]);
        }
    }
}