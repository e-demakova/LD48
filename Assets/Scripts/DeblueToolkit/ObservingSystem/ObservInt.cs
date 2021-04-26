namespace Deblue.ObservingSystem
{
    public class ObservInt : ObservLimitProperty<int>
    {
        public static explicit operator int(ObservInt i) => i.Value;
        public static explicit operator float(ObservInt i) => i.Value;

        public static ObservInt operator ++(ObservInt a)
        {
            a.Value++;
            return a;
        }
        
        public static ObservInt operator --(ObservInt a)
        {
            a.Value--;
            return a;
        }
        
        public static ObservInt operator +(ObservInt a, int b)
        {
            a.Value += b;
            return a;
        }
        
        public static ObservInt operator -(ObservInt a, int b)
        {
            a.Value -= b;
            return a;
        }
        
        public static ObservInt operator *(ObservInt a, int b)
        {
            a.Value *= b;
            return a;
        }
        
        public static ObservInt operator /(ObservInt a, int b)
        {
            a.Value /= b;
            return a;
        }

        public static bool operator >(ObservInt a, int b) => (a.Value > b);
        public static bool operator <(ObservInt a, int b) => (a.Value < b);
        
        public static bool operator >=(ObservInt a, int b) => (a.Value >= b);
        public static bool operator <=(ObservInt a, int b) => (a.Value <= b);

        public static bool operator ==(ObservInt a, int b) => (a.Value == b);
        public static bool operator !=(ObservInt a, int b) => (a.Value != b);
        
        public static bool operator >(ObservInt a, ObservInt b) => (a.Value > b.Value);
        public static bool operator <(ObservInt a, ObservInt b) => (a.Value < b.Value);
        
        public static bool operator >=(ObservInt a, ObservInt b) => (a.Value >= b.Value);
        public static bool operator <=(ObservInt a, ObservInt b) => (a.Value <= b.Value);

        public static bool operator ==(ObservInt a, ObservInt b) => (a.Value == b.Value);
        public static bool operator !=(ObservInt a, ObservInt b) => (a.Value != b.Value);

        public ObservInt(int loverLimit, int upperLimit) : base(loverLimit, upperLimit)
        {
        }
        
        public ObservInt(int value, int loverLimit, int upperLimit) : base(value, loverLimit, upperLimit)
        {
        }
        
        public ObservInt(int value) : base(value, int.MinValue, int.MaxValue)
        {
        }
    }
}
