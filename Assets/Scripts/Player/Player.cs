using UnityEngine;

using Deblue.FSM;
using Deblue.InputSystem;
using Deblue.DialogSystem;

namespace Deblue.LD48
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        public TakebleObject TakenObject { get; private set; }
        public Stairs Stairs { get; private set; }
        public float Speed => _speed;
        public bool IsHoldObject => TakenObject != null;
        public bool IsTalk { get; private set; }

        [SerializeField] private float _speed;
        [SerializeField] private Vector3 _objectTakePosition;
        [SerializeField] private Vector3 _objectPutPosition;

        private InteractiveObject _nearObject;

        private StateMachine _stateMachine = new StateMachine();
        private IStatesTable _statesTable;

        private void Awake()
        {
            var statesTable = new PlayerStateTable(_stateMachine, this);
            statesTable.Init();
            _statesTable = statesTable;
        }

        private void OnEnable()
        {
            DialogSwitcher.Events.SubscribeOnDialogStart(OnDialogStart);
            DialogSwitcher.Events.SubscribeOnDialogEnd(OnDialogEnd);

            InputReciver.SubscribeOnInput<On_Button_Down>(InteractWithObject, KeyCode.F);
        }

        private void OnDisable()
        {
            DialogSwitcher.Events.UnsubscribeOnDialogStart(OnDialogStart);
            DialogSwitcher.Events.UnsubscribeOnDialogEnd(OnDialogEnd);

            InputReciver.UnsubscribeOnInput<On_Button_Down>(InteractWithObject, KeyCode.F);
        }

        private void FixedUpdate()
        {
            _statesTable.TryChangeState();
            _stateMachine.Execute();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<InteractiveObject>(out var obj))
            {
                _nearObject = obj;
            }
            else if (other.TryGetComponent<Stairs>(out var stairs))
            {
                Stairs = stairs;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<InteractiveObject>(out var obj))
            {
                _nearObject = null;
            }
            else if (other.TryGetComponent<Stairs>(out var stairs))
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

        private void InteractWithObject(On_Button_Down context)
        {
            var takebleObj = _nearObject as TakebleObject;
            if (takebleObj != null || _nearObject == null)
            {
                TryPutObject();
            }

            if (IsHoldObject)
            {
                return;
            }

            if (takebleObj != null)
            {
                if (TryTakeObject(takebleObj))
                {
                    _nearObject = null;
                }
            }
            var objContainer = _nearObject as TakebleObjectContainer;
            if (objContainer != null)
            {
                if (TryTakeObject(takebleObj))
                {
                    _nearObject = null;
                }
            }
        }

        private bool TryTakeObject(TakebleObject obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.CanTake)
            {
                SetToHands(obj);
                return true;
            }
            return false;
        }

        private bool TryReciveObjest(TakebleObjectContainer objContainer)
        {
            if (objContainer == null)
            {
                return false;
            }
            if (objContainer.CanTake)
            {
                var obj = objContainer.Take();
                SetToHands(obj);
                return true;
            }
            return false;
        }

        private void SetToHands(TakebleObject obj)
        {
            obj.Take();
            obj.transform.position = _objectTakePosition + transform.position;
            obj.transform.SetParent(transform);
            obj.Renderer.sortingLayerID = SortingLayersData.CharactersLayer;
            obj.Renderer.sortingOrder = 10;
            TakenObject = obj;
        }

        private void PutObject(On_Button_Down context)
        {
            if (_nearObject == null && IsHoldObject)
            {
                TryPutObject();
            }
        }

        private bool TryPutObject()
        {
            if (TakenObject != null)
            {
                if (TakenObject.CanPut)
                {
                    PutToGround();
                    return true;
                }
            }
            return false;
        }

        private void PutToGround()
        {
            TakenObject.Put();
            TakenObject.transform.position = _objectPutPosition + transform.position;
            TakenObject.transform.SetParent(null);
            TakenObject.Renderer.sortingLayerID = SortingLayersData.ObjectsLayer;
            TakenObject.Renderer.sortingOrder = TakenObject.DefoultSortOrder;
            TakenObject = null;
        }
    }
}