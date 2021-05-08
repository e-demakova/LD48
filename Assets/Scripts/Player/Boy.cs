using UnityEngine;

using Deblue.FSM;
using Deblue.InputSystem;
using Deblue.DialogSystem;
using Deblue.Interactive;

namespace Deblue.LD48
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Boy : UniqMono<Boy>, IObjectTaker, IDialogStarter
    {
        [SerializeField] private float   _speed;
        [SerializeField] private Vector3 _objectTakePosition;
        [SerializeField] private Vector3 _objectPutPosition;

        private StateMachine  _stateMachine = new StateMachine();
        private IStatesTable  _statesTable;
        private TakebleObject _takenObject;

        public bool IsHoldObject => _takenObject != null;
        public string TakenObject => _takenObject == null ? null : _takenObject.Id;
        public Stairs Stairs { get; private set; }
        public Animator Animator { get; private set; }
        public bool IsTalk { get; private set; }
        public float Speed => _speed;

        public bool IsCanTakeObject
        {
            get
            {
                if (IsHoldObject)
                {
                    TryPutObject();
                }
                return !IsHoldObject;
            }
        }

        protected override void MyAwake()
        {
            Animator = GetComponent<Animator>();
            var statesTable = new BoyStateTable(_stateMachine, this);
            statesTable.Init();
            _statesTable = statesTable;
        }

        private void OnEnable()
        {
            DialogSwitcher.Events.SubscribeOnDialogStart(OnDialogStart);
            DialogSwitcher.Events.SubscribeOnDialogEnd(OnDialogEnd);
        }

        private void OnDisable()
        {
            DialogSwitcher.Events.UnsubscribeOnDialogStart(OnDialogStart);
            DialogSwitcher.Events.UnsubscribeOnDialogEnd(OnDialogEnd);
        }

        private void FixedUpdate()
        {
            _statesTable.TryChangeState();
            _stateMachine.Execute();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Stairs>(out var stairs))
            {
                Stairs = stairs;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Stairs>(out var stairs))
            {
                Stairs = null;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position + _objectPutPosition, Vector3.one * 0.02f);
            Gizmos.DrawCube(transform.position + _objectTakePosition, Vector3.one * 0.02f);
        }

        public TakebleObject TakeObject()
        {
            var obj = _takenObject;
            _takenObject = null;
            return obj;
        }

        private void OnDialogStart(Dialog_Start context)
        {
            IsTalk = true;
        }

        private void OnDialogEnd(Dialog_End context)
        {
            IsTalk = false;
        }

        private void EnterDialog(On_Button_Down context)
        {
            IsTalk = true;
            InputReciver.SubscribeOnInput<On_Button_Down>(ExitDialog, KeyCode.E);
            InputReciver.UnsubscribeOnInput<On_Button_Down>(EnterDialog, KeyCode.E);
        }

        private void ExitDialog(On_Button_Down context)
        {
            IsTalk = false;
            InputReciver.UnsubscribeOnInput<On_Button_Down>(ExitDialog, KeyCode.E);
            InputReciver.SubscribeOnInput<On_Button_Down>(EnterDialog, KeyCode.E);
        }

        public bool TryTakeObject(TakebleObject obj)
        {
            if (obj.IsCanBeTaken && IsCanTakeObject)
            {
                TakeObject(obj);
                return true;
            }
            return false;
        }

        public bool TryPutObject()
        {
            if(_takenObject == null)
            {
                return false;
            }
            if (_takenObject.CanPut)
            {
                PutToGround();
                return true;
            }
            return false;
        }

        private void PutToGround()
        {
            _takenObject.Put();
            _takenObject.transform.position = _objectPutPosition + transform.position;
            _takenObject.transform.rotation = _takenObject.DefoultRotation;
            _takenObject.transform.SetParent(null);
            _takenObject.Renderer.sortingLayerID = SortingLayersData.ObjectsLayer;
            _takenObject.Renderer.sortingOrder = _takenObject.DefoultSortOrder;
            _takenObject = null;
        }

        public bool IsContainObject(string objId)
        {
            return _takenObject?.Id == objId;
        }

        public bool TryGetObject(string objId, out TakebleObject obj)
        {
            obj = null;
            if (_takenObject?.Id == objId)
            {
                obj = _takenObject;
                return true;
            }
            return false;
        }

        public TakebleObject ReturnObject()
        {
            var obj = _takenObject;
            _takenObject = null;
            return obj;
        }

        public void TakeObject(TakebleObject obj)
        {
            obj.transform.position = _objectTakePosition + transform.position;
            obj.transform.SetParent(transform);
            obj.Renderer.sortingLayerID = SortingLayersData.CharactersLayer;
            obj.Renderer.sortingOrder = 10;
            _takenObject = obj;
        }
    }
}