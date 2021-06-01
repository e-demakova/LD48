using System;
using System.Collections.Generic;

using UnityEngine;

namespace Deblue.ObservingSystem
{
    public interface IHandlerContainer
    {
        void Clear();
    }

    public interface IReadOnlyHandler<T>
    {
        IObserver Subscribe(Action<T> action, List<IObserver> observers = null);
        void Unsubscribe(Action<T> action);
    }

    public class Handler<T> : IHandlerContainer, IReadOnlyHandler<T>
    {
        protected HashSet<Action<T>> _actions      = new HashSet<Action<T>>();
        protected List<Action<T>>    _forRemoving  = new List<Action<T>>(5);
        protected List<Action<T>>    _forAdding    = new List<Action<T>>(5);

        protected bool _isIterating;

        public IObserver Subscribe(Action<T> action, List<IObserver> observers = null)
        {
            IObserver observer;
            if (_isIterating)
            {
                observer = SafeSubscribe(action);
            }
            else
            {
                observer = FullSubscribe(action);
            }

            if (observers != null)
            {
                observers.Add(observer);
            }
            return observer;
        }

        public void Unsubscribe(Action<T> action)
        {
            if (_isIterating)
            {
                SafeUnsubscribe(action);
            }
            else
            {
                FullUnsubscribe(action);
            }
        }

        public void Clear()
        {
            RemoveCompletely();
            _forAdding.Clear();
            _actions.Clear();
        }

        public void Raise(T argument)
        {
            _isIterating = true;
            foreach (var handler in _actions)
            {
                handler.Invoke(argument);
            }
            _isIterating = false;
            RemoveCompletely();
            AddCompletely();
        }

        protected IObserver SafeSubscribe(Action<T> action)
        {
            if (_forRemoving.Contains(action))
            {
                _forRemoving.Remove(action);
            }
            if (!_actions.Contains(action) && !_forAdding.Contains(action))
            {
                _forAdding.Add(action);
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogWarningFormat("You are trying to re-subscribe a handler '{0}'.", action);
            }
#endif
            return new Observer<T>(this, action);
        }

        protected IObserver FullSubscribe(Action<T> action)
        {
            _actions.Add(action);
            return new Observer<T>(this, action);
        }

        protected void SafeUnsubscribe(Action<T> action)
        {
            if (_actions.Contains(action))
            {
                _forRemoving.Add(action);
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogWarningFormat("You are trying to unsubscribe a handler '{0}' that is not subscribed.", action);
            }
#endif
        }

        protected void FullUnsubscribe(Action<T> action)
        {
            _actions.Remove(action);
        }

        protected void RemoveCompletely()
        {
            var action = _forRemoving.GetEnumerator();
            while (action.MoveNext())
            {
                FullUnsubscribe(action.Current);
            }
            _forRemoving.Clear();
        }

        protected void AddCompletely()
        {
            var action = _forAdding.GetEnumerator();
            while (action.MoveNext())
            {
                FullSubscribe(action.Current);
            }
            _forAdding.Clear();
        }
    }

    public interface IEventSender
    {
        IObserver Subscribe<T>(Action<T> action, List<IObserver> observers = null) where T : struct;
        void Unsubscribe<T>(Action<T> action) where T : struct;
    }

    public class EventSender : IEventSender
    {
        protected Dictionary<Type, IHandlerContainer> _handlers = new Dictionary<Type, IHandlerContainer>(10);

        public IObserver Subscribe<T>(Action<T> action, List<IObserver> observers = null) where T : struct
        {
            if (!_handlers.TryGetValue(typeof(T), out var handler))
            {
                handler = new Handler<T>();
                _handlers.Add(typeof(T), handler);
            }
            return (handler as Handler<T>).Subscribe(action, observers);
        }

        public void Unsubscribe<T>(Action<T> action) where T : struct
        {
            if (_handlers.TryGetValue(typeof(T), out var handler))
            {
                (handler as Handler<T>).Unsubscribe(action);
            }
        }

        public void ClearSubscribers()
        {
            foreach (var handler in _handlers)
            {
                handler.Value.Clear();
            }
        }

        public void Raise<T>(T argument) where T : struct
        {
            if (_handlers.TryGetValue(typeof(T), out var handler))
            {
                (handler as Handler<T>).Raise(argument);
            }
        }
    }

    public static class ObserverHalper 
    {
        public static void ClearObservers(List<IObserver> observers)
        {
            for (int i = 0; i < observers.Count; i++)
            {
                observers[i].Dispose();
            }
        }
    }

    public interface IObserver : IDisposable
    {
    }

    public class Observer<T> : IObserver
    {
        private Handler<T>  _handler;
        private Action<T>   _action;

        public Observer(Handler<T> handler, Action<T> action)
        {
            _handler = handler;
            _action = action;
        }

        public void Dispose()
        {
            _handler.Unsubscribe(_action);
        }
    }
}
