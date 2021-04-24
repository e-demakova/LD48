using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Deblue.ObservingSystem
{
    public interface IHandlerContainer
    {
        void Clear();
    }

    public class Handler<T> : IHandlerContainer
    {
        protected HashSet<Action<T>> _actions      = new HashSet<Action<T>>();
        protected List<Action<T>>    _forRemoving  = new List<Action<T>>(5);
        protected List<Action<T>>    _forAdding    = new List<Action<T>>(5);

        protected bool _isIterating;

        public void Subscribe(Action<T> action)
        {
            if (_isIterating)
            {
                SafeSubscribe(action);
            }
            else
            {
                FullSubscribe(action);
            }
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

        protected void SafeSubscribe(Action<T> action)
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

        protected void FullSubscribe(Action<T> action)
        {
            _actions.Add(action);
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

    public interface IReadonlyObservList<T>
    {
        void SubscribeOnAdding(Action<Value_Added<int, T>> action);
        void SubscribeOnRemoving(Action<Value_Removed<int, T>> action);
        void SubscribeOnChanging(Action<Value_Changed<int, T>> action);

        void UnsubscribeOnAdding(Action<Value_Added<int, T>> action);
        void UnsubscribeOnRemoving(Action<Value_Removed<int, T>> action);
        void UnsubscribeOnChanging(Action<Value_Changed<int, T>> action);
    }

    public class ObservList<T> : EventSender, IReadonlyObservList<T>, IList<T>
    {
        public int Count => _list.Count;
        public int Capacity => _list.Capacity;

        public bool IsReadOnly => ((ICollection<T>)_list).IsReadOnly;

        private List<T> _list;

        public ObservList() : this(0) { }

        public ObservList(int capacity)
        {
            _list = new List<T>(capacity);
        }

        public T this[int i]
        {
            get
            {
                return _list[i];
            }
            set
            {
                Raise(new Value_Changed<int, T>(i, _list[i], value));
                _list[i] = value;
            }
        }

        public void Add(T item)
        {
            _list.Add(item);
            Raise(new Value_Added<int, T>(_list.Count, item));
        }

        public void Insert(int index, T item)
        {
            Insert(index, item);
            Raise(new Value_Added<int, T>(index, item));
        }

        public void Remove(T item)
        {
            var index = _list.IndexOf(item);
            _list.Remove(item);
            Raise(new Value_Removed<int, T>(index, item));
        }

        public void RemoveAt(int index)
        {
            var item = _list[index];
            _list.RemoveAt(index);
            Raise(new Value_Removed<int, T>(index, item));
        }


        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }
        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        bool ICollection<T>.Remove(T item)
        {
            return _list.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public void Clear()
        {
            for (int i = 0; i < _list.Count; i++)
            {
                Remove(_list[i]);
            }
            ClearSubscribers();
        }

        #region Subscribing
        public void SubscribeOnAdding(Action<Value_Added<int, T>> action)
        {
            Subscribe(action);
        }

        public void SubscribeOnRemoving(Action<Value_Removed<int, T>> action)
        {
            Subscribe(action);
        }

        public void SubscribeOnChanging(Action<Value_Changed<int, T>> action)
        {
            Subscribe(action);
        }
        #endregion

        #region Unsubscribing
        public void UnsubscribeOnAdding(Action<Value_Added<int, T>> action)
        {
            Unsubscribe(action);
        }

        public void UnsubscribeOnRemoving(Action<Value_Removed<int, T>> action)
        {
            Unsubscribe(action);
        }

        public void UnsubscribeOnChanging(Action<Value_Changed<int, T>> action)
        {
            Unsubscribe(action);
        }
        #endregion
    }

    public interface IEventSender
    {
        void Subscribe<T>(Action<T> action) where T : struct;
        void Unsubscribe<T>(Action<T> action) where T : struct;
    }

    public class EventSender : IEventSender
    {
        protected Dictionary<Type, IHandlerContainer> _handlers = new Dictionary<Type, IHandlerContainer>(10);

        public void Subscribe<T>(Action<T> action) where T : struct
        {
            if (!_handlers.TryGetValue(typeof(T), out var handler))
            {
                handler = new Handler<T>();
                _handlers.Add(typeof(T), handler);
            }
            (handler as Handler<T>).Subscribe(action);
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
}
