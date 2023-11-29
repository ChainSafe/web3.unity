using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EventEmitter.NET.Model;
using Newtonsoft.Json;

namespace EventEmitter.NET
{
    /// <summary>
    /// A class that can delegate the process of both listening for specific events (by their event id) with a static
    /// event data type and triggering events (by their event id) with any event data
    ///
    /// Event listeners subscribe to events by their eventId and by the event data type the event contains. This means
    /// event listeners are statically typed and will never receive an event that its callback cannot cast safely
    /// (this includes subclasses, interfaces and object).
    /// </summary>
    public class EventDelegator : IDisposable
    {
        private static HashSet<string> contextInstances = new HashSet<string>();

        private readonly object _cacheLock = new object();
        private Dictionary<Type, Type[]> _typeToTriggerTypes = new Dictionary<Type, Type[]>();

        public string Name { get; private set; }
        public string Context { get; private set; }

        private readonly object disposeActionLock = new object();
        private List<Action> disposeActions = new List<Action>();

        /// <summary>
        /// Create a new EventDelegator. This will create an isolated module
        /// as a context for all event listeners
        /// </summary>
        public EventDelegator() : this(Guid.NewGuid().ToString())
        {
        }

        /// <summary>
        /// Create a new EventDelegator, using a module's context to store
        /// all event listeners
        /// </summary>
        /// <param name="parent">The module to grab context from</param>
        /// <exception cref="ArgumentException">If this module's context is already being used by EventDelegator</exception>
        public EventDelegator(string contextString)
        {
            this.Name = contextString + ":event-delegator";
            this.Context = contextString;

            if (contextInstances.Contains(Context))
                throw new ArgumentException(
                    $"The context string {Context} is attempting to create a new EventDelegator that overlaps with an existing EventDelegator");
            
            contextInstances.Add(Context);
        }

        /// <summary>
        /// Listen for a given event by it's eventId and trigger the parameter-less callback. This
        /// callback will be triggered for all event data types emitted with the eventId given. 
        /// </summary>
        /// <param name="eventId">The eventId of the event to listen to</param>
        /// <param name="callback">The callback to invoke when the event is triggered</param>
        public void ListenFor(string eventId, Action callback)
        {
            ListenFor<object>(eventId, (sender, @event) =>
            {
                callback();
            });
        }
        
        /// <summary>
        /// Listen for a given event by it's eventId and event data type T
        /// </summary>
        /// <param name="eventId">The eventId of the event to listen to</param>
        /// <param name="callback">The callback to invoke when the event is triggered</param>
        /// <typeparam name="T">The type of event data the callback MUST be given</typeparam>
        public void ListenFor<T>(string eventId, EventHandler<GenericEvent<T>> callback)
        {  
            EventManager<T, GenericEvent<T>>.InstanceOf(Context).EventTriggers[eventId] += callback;
            
            OnDispose(() =>
            {
                EventManager<T, GenericEvent<T>>.InstanceOf(Context).EventTriggers[eventId] -= callback;
            });
        }

        /// <summary>
        /// Remove a specific callback that is listening to a specific event by it's eventId and event data type T
        /// </summary>
        /// <param name="eventId">The eventId of the event to stop listening to</param>
        /// <param name="callback">The callback that is unsubscribing</param>
        /// <typeparam name="T">The type of event data the callback MUST be given</typeparam>
        public void RemoveListener<T>(string eventId, EventHandler<GenericEvent<T>> callback)
        {
            EventManager<T, GenericEvent<T>>.InstanceOf(Context).EventTriggers[eventId] -= callback;
        }
        
        /// <summary>
        /// Listen for a given event by it's eventId and event data type T. When the event is triggered,
        /// stop listening for the event. This effectively ensures the callback is only invoked once.
        /// </summary>
        /// <param name="eventId">The eventId of the event to listen to</param>
        /// <param name="callback">The callback to invoke when the event is triggered. The callback will only be invoked once.</param>
        /// <typeparam name="T">The type of event data the callback MUST be given</typeparam>
        public void ListenForOnce<T>(string eventId, EventHandler<GenericEvent<T>> callback)
        {
            EventHandler<GenericEvent<T>> wrappedCallback = null;
            
            wrappedCallback = delegate(object sender, GenericEvent<T> @event)
            {
                if (callback == null)
                    return;
                RemoveListener(eventId, wrappedCallback);
                callback(sender, @event);
                callback = null;
            };
            
            EventManager<T, GenericEvent<T>>.InstanceOf(Context).EventTriggers[eventId] += wrappedCallback;
        }
        
        /// <summary>
        /// Listen for a given event by it's eventId and for a event data containing a json string. When this event
        /// is triggered, the event data containing the json string is deserialized to the given type TR and the eventId
        /// is retriggered with the new deserialized event data. 
        /// </summary>
        /// <param name="eventId">The eventId of the event to listen to</param>
        /// <param name="callback">The callback to invoked with the deserialized event data</param>
        /// <typeparam name="TR">The desired event data type that MUST be deserialized to</typeparam>
        public void ListenForAndDeserialize<TR>(string eventId, EventHandler<GenericEvent<TR>> callback)
        {
            ListenFor<TR>(eventId, callback);
            
            ListenFor<string>(eventId, delegate(object sender, GenericEvent<string> @event)
            {
                try
                {
                    //Attempt to Deserialize
                    var converted = JsonConvert.DeserializeObject<TR>(@event.EventData);

                    //When we convert, we trigger same eventId with required type TR
                    Trigger(eventId, converted);
                }
                catch (Exception e)
                {
                    //Propagate any exceptions to the event callback
                    Trigger(eventId, e);
                }
            });
        }

        /// <summary>
        /// Trigger an event by its eventId, providing a typed event data. This will invoke the registered callbacks
        /// of the event listeners listening to this eventId and looking for the given event data type T. This will
        /// also trigger event listeners looking for any sub-type of T, such as subclasses or interfaces.
        /// 
        /// This will NOT trigger event listeners looking for any parent type of T. For example, triggering with a
        /// type of IMyType will NOT trigger event listeners looking for more concrete types implementing IMyType.
        /// </summary>
        /// <param name="eventId">The eventId of the event to trigger</param>
        /// <param name="eventData">The event data to trigger the event with</param>
        /// <param name="raiseOnException">Whether to raise an exception if a listener throws an exception. If false, then all exceptions are silenced</param>
        /// <typeparam name="T">The type of the event data</typeparam>
        /// <returns>true if any event listeners were triggered, otherwise false</returns>
        public bool Trigger<T>(string eventId, T eventData, bool raiseOnException = true)
        {
            return TriggerType(eventId, eventData, typeof(T), raiseOnException);
        }

        /// <summary>
        /// Trigger an event by its eventId, providing a event data and the type of the eventData.
        /// This will invoke the registered callbacks of the event listeners listening to this eventId and looking for
        /// the given event data type "typeToTrigger". This will also trigger event listeners looking for any
        /// sub-type of the given type "typeToTrigger", such as subclasses or interfaces.
        /// 
        /// This will NOT trigger event listeners looking for any parent type. For example, triggering with a
        /// type of IMyType will NOT trigger event listeners looking for more concrete types implementing IMyType.
        /// </summary>
        /// <param name="eventId">The eventId of the event to trigger</param>
        /// <param name="eventData">The event data to trigger the event with</param>
        /// <param name="typeToTrigger">The type of the given event data and the event data type that will be triggered</param>
        /// <param name="raiseOnException">Whether to raise an exception if a listener throws an exception. If false, then all exceptions are silenced</param>
        /// <returns>true if any event listeners were triggered, otherwise false</returns>
        public bool TriggerType(string eventId, object eventData, Type typeToTrigger, bool raiseOnException = true)
        {
            Type[] allPossibleTypes;
            bool wasTriggered = false;

            lock (_cacheLock)
            {
                if (_typeToTriggerTypes.ContainsKey(typeToTrigger))
                    allPossibleTypes = _typeToTriggerTypes[typeToTrigger];
                else
                {
                    if (typeToTrigger == typeof(object))
                    {
                        // If the type of object was given, then only
                        // trigger event listeners listening to the object type explicitly
                        allPossibleTypes = new[] { typeof(object) };
                    }
                    else
                    {
                        //Find all EventFactories that inherit from type T
                        var inheritedT = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from type in assembly.GetTypes()
                            where type != typeof(object) && typeToTrigger.IsSubclassOf(type)
                            select type;

                        // Create list of types that include types inherit from type T, type T, and type object
                        allPossibleTypes = inheritedT.Concat(typeToTrigger.GetInterfaces()).Append(typeToTrigger)
                            .Append(typeof(object)).Distinct().ToArray();
                    }

                    _typeToTriggerTypes.Add(typeToTrigger, allPossibleTypes);
                }
            }

            foreach (var type in allPossibleTypes)
            {
                var genericFactoryType = typeof(EventFactory<>).MakeGenericType(type);

                var instanceProperty = genericFactoryType.GetMethod("InstanceOf");
                if (instanceProperty == null) continue;
                
                var genericFactory = instanceProperty.Invoke(null, new object[] { Context });

                var genericProviderProperty = genericFactoryType.GetProperty("Provider");
                if (genericProviderProperty == null) continue;
                
                var genericProvider = genericProviderProperty.GetValue(genericFactory);
                if (genericProvider == null) continue;
                
                MethodInfo propagateEventMethod = genericProvider.GetType().GetMethod("PropagateEvent");
                if (propagateEventMethod == null) continue;

                try
                {
                    propagateEventMethod.Invoke(genericProvider, new object[] { eventId, eventData });
                    wasTriggered = true;
                }
                catch (Exception)
                {
                    if (raiseOnException)
                        throw;
                }
            }

            return wasTriggered;
        }

        private void OnDispose(Action action)
        {
            lock (disposeActionLock)
            {
                if (this.disposeActions == null)
                    throw new ObjectDisposedException("EventDelegator", "Cannot queue dispose action if Dispose was already called");
                
                this.disposeActions.Add(action);
            }
        }

        public void Dispose()
        {
            Action[] actions = new Action[this.disposeActions.Count];
            lock (disposeActionLock)
            {
                this.disposeActions.CopyTo(actions);
                this.disposeActions = null;
            }

            this._typeToTriggerTypes.Clear();
            this._typeToTriggerTypes = null;
            foreach (var action in actions)
            {
                action();
            }

            contextInstances.Remove(Context);
        }
    }
}
