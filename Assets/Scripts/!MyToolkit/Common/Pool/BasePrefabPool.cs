using System.Collections.Generic;

using UnityEngine;

using Deblue.ObservingSystem.Events;

namespace Deblue.Pool
{
    public abstract class BasePrefabPool<T> : IPool<T>, IPrefabPool<T> where T : PoolItem
    {
        protected Queue<T> _items        = new Queue<T>(15);
        protected List<T>  _itemsInScene = new List<T>(15);

        protected Transform _parent;

        public void InstantiateObjects(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GetNewItem();
            }
            ReturnAll();
        }

        public abstract T GetNewItem();

        public T GetItem()
        {
            if (_items.Count > 0)
            {
                var item = _items.Dequeue();
                StartObserveItem(item);
                return item;
            }
            return GetNewItem();
        }

        public void ReturnToPool(T item)
        {
            item.DeInit();
            item.UnsubscribeOnLifeEnd(ReturnToPool);
            _items.Enqueue(item);
        }

        public void ReturnAll()
        {
            for (int i = 0; i < _itemsInScene.Count; i++)
            {
                ReturnToPool(_itemsInScene[i]);
            }
        }

        protected void StartObserveItem(T item)
        {
            _itemsInScene.Add(item);
            item.SubscribeOnLifeEnd(ReturnToPool);
        }

        protected void ReturnToPool(Pool_Item_Life_End context)
        {
            ReturnToPool((T)context.Item);
        }
    }
}