using System;
using System.Collections;
using System.Collections.Generic;

namespace Deblue.ObservingSystem
{
    public interface IReadonlyObservDictionary<Tkey, TValue>
    {
        IObserver SubscribeOnAdding(Action<Value_Added<Tkey, TValue>> action, List<IObserver> observers = null);
        IObserver SubscribeOnRemoving(Action<Value_Removed<Tkey, TValue>> action, List<IObserver> observers = null);
        IObserver SubscribeOnChanging(Action<Value_Changed<Tkey, TValue>> action, List<IObserver> observers = null);

        void UnsubscribeOnAdding(Action<Value_Added<Tkey, TValue>> action);
        void UnsubscribeOnRemoving(Action<Value_Removed<Tkey, TValue>> action);
        void UnsubscribeOnChanging(Action<Value_Changed<Tkey, TValue>> action);
    }

    [Serializable]
    public class ObservDictionary<TKey, TValue> : EventSender, IReadonlyObservDictionary<TKey, TValue>, IDictionary<TKey, TValue>
    {
        public ObservDictionary() : this(0) { }

        public ObservDictionary(int capacity)
        {
            _dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        private Dictionary<TKey, TValue> _dictionary;

        public ICollection<TKey> Keys => _dictionary.Keys;

        public ICollection<TValue> Values => _dictionary.Values;

        public int Count => _dictionary.Count;

        public bool IsReadOnly => false;

        public TValue this[TKey key]
        {
            get
            {
                return _dictionary[key];
            }
            set
            {
                var oldValue = _dictionary[key];
                _dictionary[key] = value;
                Raise(new Value_Changed<TKey, TValue>(key, oldValue, value));
            }
        }

        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
            Raise(new Value_Added<TKey, TValue>(key, value));
        }

        public bool Remove(TKey key)
        {
            var value = _dictionary[key];
            _dictionary.Remove(key);
            Raise(new Value_Removed<TKey, TValue>(key, value));
            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public void Clear()
        {
            foreach (var item in _dictionary)
            {
                Raise(new Value_Removed<TKey, TValue>(item.Key, item.Value));
            }
            _dictionary.Clear();
            ClearSubscribers();
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.ContainsKey(item.Key) && _dictionary.ContainsValue(item.Value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            Array.Resize(ref array, arrayIndex + _dictionary.Count);

            int i = arrayIndex;
            foreach (var item in _dictionary)
            {
                array[i] = item;
                i++;
            }
        }

        #region Subscribing
        public IObserver SubscribeOnAdding(Action<Value_Added<TKey, TValue>> action, List<IObserver> observers = null)
        {
            return Subscribe(action, observers);
        }

        public IObserver SubscribeOnRemoving(Action<Value_Removed<TKey, TValue>> action, List<IObserver> observers = null)
        {
            return Subscribe(action, observers);
        }

        public IObserver SubscribeOnChanging(Action<Value_Changed<TKey, TValue>> action, List<IObserver> observers = null)
        {
            return Subscribe(action, observers);
        }
        #endregion

        #region Unsubscribing
        public void UnsubscribeOnAdding(Action<Value_Added<TKey, TValue>> action)
        {
            Unsubscribe(action);
        }

        public void UnsubscribeOnRemoving(Action<Value_Removed<TKey, TValue>> action)
        {
            Unsubscribe(action);
        }

        public void UnsubscribeOnChanging(Action<Value_Changed<TKey, TValue>> action)
        {
            Unsubscribe(action);
        }
        #endregion
    }
}
