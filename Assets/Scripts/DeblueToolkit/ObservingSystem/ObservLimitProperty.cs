using System;

namespace Deblue.ObservingSystem
{
    public interface IReadonlyObservLimitProperty<T> where T : IComparable, IComparable<T>, IEquatable<T>
    {
        T Value { get; }
        T LowerLimit { get; }
        T UpperLimit { get; }

        void SubscribeOnChanging(Action<Property_Changed<T>> action);
        void SubscribeOnLoverLimit(Action<Property_Reached_Lower_Limit> action);
        void SubscribeOnUpperLimit(Action<Property_Reached_Upper_Limit> action);

        void UnsubscribeOnChanging(Action<Property_Changed<T>> action);
        void UnsubscribeOnLoverLimit(Action<Property_Reached_Lower_Limit> action);
        void UnsubscribeOnUpperLimit(Action<Property_Reached_Upper_Limit> action);
    }

    public class ObservLimitProperty<T> : EventSender, IReadonlyObservLimitProperty<T>, IDisposable where T : IComparable, IComparable<T>, IEquatable<T>
    {
        private T _value;
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

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                var oldValue = _value;
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
                Raise(new Property_Changed<T>(oldValue, _value, _lowerLimit, _upperLimit));
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
                Raise(new Property_Changed<T>(_value, _value, _lowerLimit, _upperLimit));
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
                Raise(new Property_Changed<T>(_value, _value, _lowerLimit, _upperLimit));
            }
        }

        public void Dispose()
        {
            ClearSubscribers();
        }

        public override string ToString() => Value.ToString();

        #region Subscribing
        public void SubscribeOnChanging(Action<Property_Changed<T>> action)
        {
            Subscribe(action);
        }

        public void SubscribeOnLoverLimit(Action<Property_Reached_Lower_Limit> action)
        {
            Subscribe(action);
        }

        public void SubscribeOnUpperLimit(Action<Property_Reached_Upper_Limit> action)
        {
            Subscribe(action);
        }
        #endregion

        #region Unsubscribing
        public void UnsubscribeOnChanging(Action<Property_Changed<T>> action)
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
