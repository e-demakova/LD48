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

        protected abstract bool CanHighlight { get; }

        public SpriteRenderer Renderer { get; private set; }
        public int DefoultSortOrder { get; private set; }

        protected Collider2D _collider;
        protected Player     _player;

        protected bool _isHighlight;
        protected bool _isTaken;

        protected void Awake()
        {
            _collider = GetComponent<Collider2D>();
            Renderer = GetComponent<SpriteRenderer>();
            DefoultSortOrder = Renderer.sortingOrder;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                TryHilight();
                _player = player;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                _player = null;
                StopHighlight();
            }
        }

        protected void TryHilight()
        {
            if (CanHighlight)
            {
                Highlight();
            }
        }

        protected abstract void Highlight();
        protected abstract void StopHighlight();
    }

    public abstract class TakebleObject : InteractiveObject, ITakebleObject
    {
        public abstract bool CanPut { get; }
        public abstract bool CanTake { get; }

        public ITakebleObject Take(Vector3 takePosition)
        {
            _isTaken = true;
            StopHighlight();
            return this;
        }

        public void Put(Vector3 putPosition)
        {
            _isTaken = false;
            TryHilight();
        }
    }

    public abstract class TakebleObjectContainer : InteractiveObject, ITakebleObjectContainer
    {
        public abstract bool CanReturn { get; }
        public abstract bool CanTake { get; }

        public abstract ITakebleObject Take(Vector3 takePosition);
        public abstract void Return();
    }
}