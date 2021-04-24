namespace Deblue.ObservingSystem
{
    public readonly struct Property_Changed<T>
    {
        public readonly T OldValue;
        public readonly T NewValue;

        public readonly T LowerLimit;
        public readonly T UpperLimit;

        public Property_Changed(T oldValue, T newValue, T lowerLimit, T upperLimit)
        {
            OldValue = oldValue;
            NewValue = newValue;

            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
        }
    }
    
    public readonly struct Property_Reached_Lower_Limit
    {
    }

    public readonly struct Property_Reached_Upper_Limit
    {
    }

    public readonly struct Value_Changed<TKey, TValue>
    {
        public readonly TKey   Key;
        public readonly TValue OldValue;
        public readonly TValue NewValue;

        public Value_Changed(TKey key, TValue oldValue, TValue newValue)
        {
            Key = key;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public readonly struct Value_Removed<TKey, TValue>
    {
        public readonly TKey   Index;
        public readonly TValue Value;

        public Value_Removed(TKey key, TValue value)
        {
            Index = key;
            Value = value;
        }
    }

    public readonly struct Value_Added<TKey, TValue>
    {
        public readonly TKey   Key;
        public readonly TValue Value;

        public Value_Added(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
