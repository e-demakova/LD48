namespace Deblue.ObservingSystem
{
    [System.Serializable]
    public class ObservFloat : ObservLimitProperty<float>
    {
        public static implicit operator float(ObservFloat i) => i.Value;
        public static explicit operator int(ObservFloat i) => (int)i.Value;

        public static ObservFloat operator ++(ObservFloat a)
        {
            a.Value++;
            return a;
        }

        public static ObservFloat operator --(ObservFloat a)
        {
            a.Value--;
            return a;
        }

        public static ObservFloat operator +(ObservFloat a, float b)
        {
            a.Value += b;
            return a;
        }

        public static ObservFloat operator -(ObservFloat a, float b)
        {
            a.Value -= b;
            return a;
        }

        public static ObservFloat operator *(ObservFloat a, float b)
        {
            a.Value *= b;
            return a;
        }

        public static ObservFloat operator /(ObservFloat a, float b)
        {
            a.Value /= b;
            return a;
        }

        public static bool operator >(ObservFloat a, float b) => (a.Value > b);
        public static bool operator <(ObservFloat a, float b) => (a.Value < b);

        public static bool operator >=(ObservFloat a, float b) => (a.Value >= b);
        public static bool operator <=(ObservFloat a, float b) => (a.Value <= b);

        public static bool operator ==(ObservFloat a, int b) => (a.Value == b);
        public static bool operator !=(ObservFloat a, int b) => (a.Value != b);

        public static bool operator >(ObservFloat a, ObservFloat b) => (a.Value > b.Value);
        public static bool operator <(ObservFloat a, ObservFloat b) => (a.Value < b.Value);

        public static bool operator >=(ObservFloat a, ObservFloat b) => (a.Value >= b.Value);
        public static bool operator <=(ObservFloat a, ObservFloat b) => (a.Value <= b.Value);

        public static bool operator ==(ObservFloat a, ObservFloat b) => (a.Value == b.Value);
        public static bool operator !=(ObservFloat a, ObservFloat b) => (a.Value != b.Value);

        public ObservFloat(float loverLimit, float upperLimit) : base(loverLimit, upperLimit)
        {
        }

        public ObservFloat(float value, float loverLimit, float upperLimit) : base(value, loverLimit, upperLimit)
        {
        }

        public ObservFloat(float value) : base(value, float.MinValue, float.MaxValue)
        {
        }

        public override bool Equals(object obj)
        {
            return obj is ObservInt obsInt &&
                   Value == obsInt.Value &&
                   LowerLimit == obsInt.LowerLimit &&
                   UpperLimit == obsInt.UpperLimit;
        }

        public override int GetHashCode()
        {
            int hashCode = -1304306776;
            hashCode = hashCode * -1521134295 + _value.GetHashCode();
            hashCode = hashCode * -1521134295 + LowerLimit.GetHashCode();
            hashCode = hashCode * -1521134295 + UpperLimit.GetHashCode();
            return hashCode;
        }
    }
}
