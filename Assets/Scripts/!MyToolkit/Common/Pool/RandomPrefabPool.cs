using UnityEngine;

namespace Deblue.Pool
{
    public class RandomPrefabPool<T> : BasePrefabPool<T> where T : PoolItem
    {
        protected T[] _prefabs;

        protected int RandomIndex => Random.Range(0, _prefabs.Length);

        public RandomPrefabPool(T[] prefabs, Transform parent)
        {
            _prefabs = prefabs;
            _parent = parent;
        }

        public override T GetNewItem()
        {
            var prefab = _prefabs[RandomIndex];
            var item = Object.Instantiate(prefab, _parent);
            StartObserveItem(item);
            return item;
        }
    }
}