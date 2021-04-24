using UnityEngine;

namespace Deblue.LD48
{
    public abstract class InteractiveOneState : InteractiveObject
    {
        [SerializeField] protected SpritePair _sprites;

        protected sealed override void OverHighlight()
        {
            _renderer.sprite = _sprites.Standart;
        }
        
        protected sealed override void Highlight()
        {
            _renderer.sprite = _sprites.Highlight;
        }
    }
}