using Deblue.Pool;

namespace Deblue.ObservingSystem.Events
{ 
    public readonly struct Pool_Item_Life_End
    {
        public readonly PoolItem Item;

        public Pool_Item_Life_End(PoolItem item)
        {
            Item = item;
        }
    }
}