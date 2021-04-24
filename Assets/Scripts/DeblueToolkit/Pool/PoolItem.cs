using System;

using UnityEngine;

using Deblue.ObservingSystem;
using Deblue.ObservingSystem.Events;

namespace Deblue.Pool
{
    public interface IPoolItem
    {
        void DeInit();
    }

    public abstract class PoolItem : MonoBehaviour, IPoolItem
    {
        protected EventSender _events;

        public void SubscribeOnLifeEnd(Action<Pool_Item_Life_End> action)
        {
            _events.Subscribe(action);
        }
        
        public void UnsubscribeOnLifeEnd(Action<Pool_Item_Life_End> action)
        {
            _events.Unsubscribe(action);
        }

        public abstract void DeInit();

        protected virtual void OnLifeEnd()
        {
            _events.Raise(new Pool_Item_Life_End(this));
        }
    }
}