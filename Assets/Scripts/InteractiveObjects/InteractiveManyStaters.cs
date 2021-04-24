using UnityEngine;

namespace Deblue.LD48
{
    public abstract class InteractiveManyStaters : InteractiveObject
    {
        [SerializeField] protected SpritePair[] _sprites;
        [SerializeField] protected int _stateIndex;

        protected sealed override void OverHighlight()
        {
            _renderer.sprite = _sprites[_stateIndex].Standart;
        }

        protected sealed override void Highlight()
        {
            _renderer.sprite = _sprites[_stateIndex].Highlight;
        }
    }
}