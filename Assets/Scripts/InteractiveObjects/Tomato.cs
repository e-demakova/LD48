namespace Deblue.LD48
{
    public class Tomato : InteractiveManyStaters
    {
        public override bool CanPut()
        {
            return false;
        }

        public override bool CanTake()
        {
            return false;
        }

        protected override bool CanHighlight()
        {
            return true;
        }
    }
}