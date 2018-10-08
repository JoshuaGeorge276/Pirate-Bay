using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Core
{
    public class GameEvent { }

    public class EventSystem : SingletonBehaviour<EventSystem>
    {
        public delegate void EventDelegate<T>(T e) where T : GameEvent;
        private delegate void EventDelegate(GameEvent e);

        private readonly Dictionary<System.Type, EventDelegate> _delegates = new Dictionary<Type, EventDelegate>();
        private readonly Dictionary<System.Delegate, EventDelegate> _delegateLookup = new Dictionary<Delegate, EventDelegate>();

        public void Subscribe<T>(EventDelegate<T> a_del) where T : GameEvent
        {
            if (_delegateLookup.ContainsKey(a_del))
                return;

            EventDelegate internalDelegate = (e) => a_del((T)e);
            _delegateLookup[a_del] = internalDelegate;

            if (_delegates.TryGetValue(typeof(T), out EventDelegate tempDel))
            {
                _delegates[typeof(T)] = tempDel += internalDelegate;
            }
            else
            {
                tempDel = internalDelegate;
                _delegates.Add(typeof(T), tempDel);
            }
        }

        public void Unsubscribe<T>(EventDelegate<T> a_del) where T : GameEvent
        {
            if (_delegateLookup.TryGetValue(a_del, out EventDelegate internalDelegate))
            {
                if (_delegates.TryGetValue(typeof(T), out EventDelegate tempDel))
                {
                    tempDel -= internalDelegate;
                    if (tempDel == null)
                    {
                        _delegates.Remove(typeof(T));
                    }
                    else
                    {
                        _delegates[typeof(T)] = tempDel;
                    }
                }

                _delegateLookup.Remove(a_del);
            }
        }

        public void Raise(GameEvent e)
        {
            if (e == null)
                return;

            if (_delegates.TryGetValue(e.GetType(), out EventDelegate del))
            {
                del.Invoke(e);
            }
        }
    }
}
