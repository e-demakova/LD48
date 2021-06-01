using System.Collections.Generic;

using UnityEngine;

namespace Deblue.Pool
{
    public class PrefabPool<T> : BasePrefabPool<T> where T : PoolItem
    {
        protected T _prefab;

        public PrefabPool(T prefab, Transform parent, int count)
        {
            _prefab = prefab;
            _parent = parent;
        }

        public override T GetNewItem()
        {
            var item = Object.Instantiate(_prefab, _parent);
            StartObserveItem(item);
            return item;
        }
    }
}