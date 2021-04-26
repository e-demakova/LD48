namespace Deblue.ObservingSystem
{
    public class ObservFloat : ObservLimitProperty<float>
    {
        public static explicit operator int(ObservFloat i) => (int)i.Value;
        public static explicit operator float(ObservFloat i) => i.Value;

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
        
        public static bool operator >(ObservFloat a, ObservFloat b) => (a.Value > b.Value);
        public static bool operator <(ObservFloat a, ObservFloat b) => (a.Value < b.Value);
        
        public static bool operator >=(ObservFloat a, ObservFloat b) => (a.Value >= b.Value);
        public static bool operator <=(ObservFloat a, ObservFloat b) => (a.Value <= b.Value);


        public ObservFloat(float loverLimit, float upperLimit) : base(loverLimit, upperLimit)
        {
        }
        
        public ObservFloat(float value, float loverLimit, float upperLimit) : base(value, loverLimit, upperLimit)
        {
        }
        
        public ObservFloat(float value) : base(value, float.MinValue, float.MaxValue)
        {
        }
    }
}
