namespace Deblue.LD48
{
    public class Tomato : TakebleObject
    {
        public override bool CanPut => throw new System.NotImplementedException();
        public override bool CanTake => throw new System.NotImplementedException();
        protected override bool CanHighlight => true;

        protected override void Highlight()
        {
            throw new System.NotImplementedException();
        }

        protected override void StopHighlight()
        {
            throw new System.NotImplementedException();
        }
    }
}