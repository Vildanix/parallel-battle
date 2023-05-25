using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Locator : ScriptableObject
{
    public enum INSTANCE_TYPE { SINGLETON, TRANSIENT }
            
            private static Dictionary<Type, List<object>> repository = new();
            private static Dictionary<Type, List<(Action<object>, bool)>> listeners = new();
            private static Dictionary<Type, (Func<object>, INSTANCE_TYPE)> factories = new();
    
            public static void Get<T>(Action<object> onChange, bool keepNotificationsAfterResolve = false) where T:class
            {
                var t = typeof(T);
                if (!listeners.ContainsKey(t))
                {
                    listeners[t] = new();
                }
    
                bool addListener = true;
                if (factories.ContainsKey(t) && factories[t].Item2 == INSTANCE_TYPE.TRANSIENT)
                {
                    AddToRepository<T>(factories[t].Item1());
                    onChange(repository[t] as T);
                    addListener = keepNotificationsAfterResolve;
                }
                else if (repository.ContainsKey(t))
                {
                    onChange(repository[t]);
                    addListener = keepNotificationsAfterResolve;
                }
                
                if (addListener) listeners[t].Add((onChange, keepNotificationsAfterResolve));
            }
    
            public static void Assign<T>(object value)
            {
                var t = typeof(T);
                if (repository.ContainsKey(t) && repository[t].Count > 0 &&
                    factories.ContainsKey(t) && factories[t].Item2 == INSTANCE_TYPE.SINGLETON)
                {
                    throw new InvalidOperationException($"Object instance is already registered for {typeof(T)}");
                }
                
                AddToRepository<T>(value);
    
                if (listeners.ContainsKey(t))
                {
                    var localListeners = listeners[t].ToList();
                    foreach (var (onChange, keepNotifications) in localListeners)
                    {
                        onChange(value);
                        if (!keepNotifications)
                        {
                            listeners[t].Remove((onChange, keepNotifications));
                        }
                    }
                }
            }
    
            private static void AddToRepository<T>(object value)
            {
                var t = typeof(T);
                if (!repository.ContainsKey(t))
                {
                    repository[t] = new() {value};
                    return;
                }
    
                repository[t].Add(value);
            }
            
            public static void Unsubscribe<T>(Action<object> onChange)
            {
                var t = typeof(T);
                if (listeners.ContainsKey(t))
                {
                    var (listener, keepNotifications) = listeners[t].FirstOrDefault(l => l.Item1 == onChange);
                    if (listener != null)
                    {
                        listeners[t].Remove((listener, keepNotifications));
                    }
                }
            }
        
}
