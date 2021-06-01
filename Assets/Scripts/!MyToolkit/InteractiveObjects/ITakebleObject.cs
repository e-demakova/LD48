namespace Deblue.Interactive
{
    public interface IInteractObject
    {
        bool CanHighlight(IObjectTaker taker);
        bool TryHighlight(IObjectTaker taker);
        void Highlight();
        void StopHighlight();
    }

    public interface IReactionObject : IInteractObject
    {
        bool CanReact(IObjectTaker taker);
        bool TryReact(IObjectTaker taker);
        void React(IObjectTaker taker);
    }

    public interface ITakebleObject : IInteractObject
    {
        string Id { get; }
        bool CanPut { get; }

        bool IsCanBeTaken { get; }

        TakebleObject Take();

        void Put();
    }

    public interface ITakebleObjectContainer : IInteractObject
    {
        bool CanTake(IObjectTaker taker);
        bool TryTake(IObjectTaker taker, out TakebleObject obj);
        TakebleObject Take();

        bool CanReturn(string objId);
        void Return(TakebleObject obj);
    }
}
