using UnityEngine;

namespace Deblue.LD48
{
    public interface ITakebleObject
    {
        bool CanPut { get; }

        bool CanTake { get; }

        ITakebleObject Take(Vector3 takePosition);

        void Put(Vector3 putPosition);
    }

    public interface ITakebleObjectContainer
    {
        bool CanReturn { get; }

        bool CanTake { get; }

        ITakebleObject Take(Vector3 takePosition);

        void Return();
    }
}
