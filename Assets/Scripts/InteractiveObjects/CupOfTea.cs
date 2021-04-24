namespace Deblue.LD48
{
    public class CupOfTea : InteractiveManyStaters
    {
        public override bool CanPut()
        {
            return _isTaken;
        }

        public override bool CanTake()
        {
            return CanHighlight();
        }

        protected override bool CanHighlight()
        {
            return !_isTaken;
        }
    }
}