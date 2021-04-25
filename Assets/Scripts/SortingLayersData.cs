using UnityEngine;

namespace Deblue.LD48
{
    public class SortingLayersData : UniqMono<SortingLayersData>
    {
        public static int CharactersLayer;
        public static int ObjectsLayer;

        protected override void MyAwake()
        {
            CharactersLayer = SortingLayer.NameToID("Characters");
            ObjectsLayer = SortingLayer.NameToID("Objects");
        }
    }
}