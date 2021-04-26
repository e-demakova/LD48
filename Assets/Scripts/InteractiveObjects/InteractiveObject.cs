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

        [SerializeField] protected SpriteRenderer _keyView;

        protected Collider2D _collider;
        protected Player     _player;
        protected bool       _isHighlight;
        protected bool       _isTaken;

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
                _player = player;
                _player.AddObject(this);
                TryHilight();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                _player.RemoveObject(this);
                _keyView.enabled = false;
                StopHighlight();
            }
        }

        protected void TryHilight()
        {
            if (CanHighlight)
            {
                _keyView.enabled = true;
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

        public TakebleObject Take()
        {
            _isTaken = true;
            StopHighlight();
            return this;
        }

        public void Put()
        {
            _isTaken = false;
            TryHilight();
        }
    }

    public abstract class TakebleObjectContainer : InteractiveObject, ITakebleObjectContainer
    {
        public abstract bool CanReturn { get; }
        public abstract bool CanTake { get; }

        public abstract TakebleObject Take();
        public abstract void Return();
    }
}