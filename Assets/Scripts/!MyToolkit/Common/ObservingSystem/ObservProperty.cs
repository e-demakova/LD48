using System;
using System.Collections.Generic;

namespace Deblue.ObservingSystem
{
    public interface IReadonlyObservLimitProperty<T> where T : IComparable, IComparable<T>, IEquatable<T>
    {
        T Value { get; }
        T LowerLimit { get; }
        T UpperLimit { get; }

        IObserver SubscribeOnChanging(Action<Limited_Property_Changed<T>> action, List<IObserver> observers = null);
        IObserver SubscribeOnLoverLimit(Action<Property_Reached_Lower_Limit> action, List<IObserver> observers = null);
        IObserver SubscribeOnUpperLimit(Action<Property_Reached_Upper_Limit> action, List<IObserver> observers = null);

        void UnsubscribeOnChanging(Action<Limited_Property_Changed<T>> action);
        void UnsubscribeOnLoverLimit(Action<Property_Reached_Lower_Limit> action);
        void UnsubscribeOnUpperLimit(Action<Property_Reached_Upper_Limit> action);
    }

    public interface IReadonlyObservProperty<T> where T : IComparable, IComparable<T>, IEquatable<T>
    {
        T Value { get; }

        IObserver SubscribeOnChanging(Action<Property_Changed<T>> action, List<IObserver> observers = null);
        void UnsubscribeOnChanging(Action<Property_Changed<T>> action);
    }

    public abstract class BaseObservProperty<T> : EventSender, IDisposable where T : IComparable, IComparable<T>, IEquatable<T>
    {
        public abstract T Value { get; set; }

        protected T _value;

        public BaseObservProperty() : this(default(T))
        {
        }

        public BaseObservProperty(T value)
        {
            _value = value;
        }

        public override string ToString() => Value.ToString();

        public override bool Equals(object obj)
        {
            return obj is BaseObservProperty<T> property &&
                   Value.Equals(property.Value);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public void Dispose()
        {
            ClearSubscribers();
        }
    }

    public class ObservProperty<T> : BaseObservProperty<T>, IReadonlyObservProperty<T> where T : IComparable, IComparable<T>, IEquatable<T>
    {
        public ObservProperty() : this(default(T))
        {
        }

        public ObservProperty(T value)
        {
            _value = value;
        }

        public override sealed T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                Raise(new Property_Changed<T>(_value));
            }
        }

        public IObserver SubscribeOnChanging(Action<Property_Changed<T>> action, List<IObserver> observers = null)
        {
            return Subscribe(action, observers);
        }

        public void UnsubscribeOnChanging(Action<Property_Changed<T>> action)
        {
            Unsubscribe(action);
        }
    }

    [Serializable]
    public class ObservLimitProperty<T> : BaseObservProperty<T>, IReadonlyObservLimitProperty<T> where T : IComparable, IComparable<T>, IEquatable<T>
    {
        private T _lowerLimit;
        private T _upperLimit;

        public ObservLimitProperty(T loverLimit, T upperLimit) : this(default(T), loverLimit, upperLimit)
        {
        }

        public ObservLimitProperty(T value, T loverLimit, T upperLimit)
        {
            _value = value;
            _lowerLimit = loverLimit;
            _upperLimit = upperLimit;
        }

        public override sealed T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value.CompareTo(_lowerLimit) <= 0)
                {
                    _value = _lowerLimit;
                    Raise(new Property_Reached_Lower_Limit());
                }
                else if (value.CompareTo(_upperLimit) >= 0)
                {
                    _value = _upperLimit;
                    Raise(new Property_Reached_Upper_Limit());
                }
                else
                {
                    _value = value;
                }
                Raise(new Limited_Property_Changed<T>(_value, _lowerLimit, _upperLimit));
            }
        }

        public T LowerLimit
        {
            get
            {
                return _lowerLimit;
            }
            set
            {
                _lowerLimit = value;
                Raise(new Limited_Property_Changed<T>(_value, _lowerLimit, _upperLimit));
            }
        }

        public T UpperLimit
        {
            get
            {
                return _upperLimit;
            }
            set
            {
                _upperLimit = value;
                Raise(new Limited_Property_Changed<T>(_value, _lowerLimit, _upperLimit));
            }
        }

        #region Subscribing
        public IObserver SubscribeOnChanging(Action<Limited_Property_Changed<T>> action, List<IObserver> observers = null)
        {
            return Subscribe(action, observers);
        }

        public IObserver SubscribeOnLoverLimit(Action<Property_Reached_Lower_Limit> action, List<IObserver> observers = null)
        {
            return Subscribe(action, observers);
        }

        public IObserver SubscribeOnUpperLimit(Action<Property_Reached_Upper_Limit> action, List<IObserver> observers = null)
        {
            return Subscribe(action, observers);
        }
        #endregion

        #region Unsubscribing
        public void UnsubscribeOnChanging(Action<Limited_Property_Changed<T>> action)
        {
            Unsubscribe(action);
        }

        public void UnsubscribeOnLoverLimit(Action<Property_Reached_Lower_Limit> action)
        {
            Unsubscribe(action);
        }

        public void UnsubscribeOnUpperLimit(Action<Property_Reached_Upper_Limit> action)
        {
            Unsubscribe(action);
        }
        #endregion
    }
}
