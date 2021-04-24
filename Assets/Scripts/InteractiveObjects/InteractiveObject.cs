using UnityEngine;

namespace Deblue.LD48
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class InteractiveObject : MonoBehaviour
    {
        [System.Serializable]
        protected struct SpritePair
        {
            public Sprite Standart;
            public Sprite Highlight;
        }

        protected Player _possibleOwner;
        protected SpriteRenderer _renderer;

        protected void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        protected bool _isHighlight;
        protected bool _isTaken;

        public void OnPlayerEnter(Player player)
        {
            if (CanHighlight())
            {
                Highlight();
            }
            _possibleOwner = player;
        }

        public void OnPlayerExit()
        {
            _possibleOwner = null;
            OverHighlight();
        }

        public abstract bool CanTake();
        public void Take()
        {
            _isTaken = true;
            transform.SetParent(_possibleOwner.transform);
        }

        public abstract bool CanPut();
        public void Put()
        {
            _isTaken = false;
            transform.SetParent(null);
        }

        protected abstract bool CanHighlight();
        protected abstract void Highlight();
        protected abstract void OverHighlight();
    }
}