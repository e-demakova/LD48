using UnityEngine;

namespace Deblue.Pool
{
    public interface IPool<T> where T : IPoolItem
    {
        T GetItem();
        T GetNewItem();
        void ReturnToPool(T spawnObject);
    }
    
    public interface IPrefabPool<T> : IPool<T> where T : MonoBehaviour, IPoolItem 
    {
        void InstantiateObjects(int count);
        void ReturnAll();
    }
}