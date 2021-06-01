using UnityEngine;

using Deblue.ObservingSystem;

namespace Deblue.Interactive
{
    public readonly struct Interact_Object_Updated
    {
        public readonly InteractObject Obj;
        
        public Interact_Object_Updated(InteractObject obj)
        {
            Obj = obj;
        }
    }

    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class InteractObject : MonoBehaviour, IInteractObject
    {
        [System.Serializable]
        protected struct SpritePair
        {
            public Sprite Standart;
            public Sprite Highlight;
        }

        protected Handler<Interact_Object_Updated> _updated = new Handler<Interact_Object_Updated>();

        protected Collider2D _collider;
        protected bool       _isHighlight;
        protected bool       _isTaken;
        protected bool       _isPlayerNear;

        [SerializeField] protected SpriteRenderer _keyView;
        [SerializeField] protected string _id;

        public string Id => _id;
        public SpriteRenderer Renderer { get; private set; }
        public int DefoultSortOrder { get; private set; }
        public Quaternion DefoultRotation { get; private set; }
        public IReadOnlyHandler<Interact_Object_Updated> Updated => _updated;

        protected void Awake()
        {
            _collider = GetComponent<Collider2D>();
            Renderer = GetComponent<SpriteRenderer>();
            DefoultSortOrder = Renderer.sortingOrder;
            DefoultRotation = transform.rotation;
            MyAwake();
        }

        protected virtual void MyAwake()
        {
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IObjectTaker>(out var taker))
            {
                _updated.Raise(new Interact_Object_Updated(this));
                _isPlayerNear = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<IObjectTaker>(out var taker))
            {
                _updated.Raise(new Interact_Object_Updated(this));
                _isPlayerNear = false;
            }
        }

        public bool TryHighlight(IObjectTaker taker)
        {
            if (CanHighlight(taker) && _isPlayerNear)
            {
                _keyView.enabled = true;
                Highlight();
                return true;
            }
            else
            {
                StopHighlight();
                return false;
            }
        }

        public abstract bool CanHighlight(IObjectTaker taker);
        public abstract void Highlight();
        public abstract void StopHighlight();
    }

    public abstract class TakebleObject : InteractObject, ITakebleObject
    {
        public abstract bool CanPut { get; }
        public abstract bool IsCanBeTaken { get; }

        public TakebleObject Take()
        {
            _isTaken = true;
            _updated.Raise(new Interact_Object_Updated(this));
            return this;
        }

        public void Put()
        {
            _isTaken = false;
            _updated.Raise(new Interact_Object_Updated(this));
        }
    }

    public abstract class TakebleObjectContainer : InteractObject, ITakebleObjectContainer
    {
        [SerializeField] protected string _conteinedObjectId;

        public bool TryTake(IObjectTaker taker, out TakebleObject obj)
        {
            obj = null;
            if (CanTake(taker))
            {
                obj = Take();
                _updated.Raise(new Interact_Object_Updated(this));
                return true;
            }
            return false;
        }

        public abstract bool CanTake(IObjectTaker taker);
        public abstract TakebleObject Take();

        public abstract bool CanReturn(string objId);
        public abstract void Return(TakebleObject obj);
    }
}