namespace Deblue.LD48
{
    public interface IReactionObject
    {
        bool CanReact { get; }

        void React();
    }

    public interface ITakebleObject
    {
        bool CanPut { get; }

        bool CanTake { get; }

        TakebleObject Take();

        void Put();
    }

    public interface ITakebleObjectContainer
    {
        bool CanReturn { get; }

        bool CanTake { get; }

        TakebleObject Take();

        void Return();
    }
}
