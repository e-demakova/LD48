using System.Collections.Generic;

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

        private List<InteractiveObject> _nearObjects = new List<InteractiveObject>(10);

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
            /*if (other.TryGetComponent<InteractiveObject>(out var obj))
            {
                _nearObjects.Add(obj);
            }
            else*/
            if (other.TryGetComponent<Stairs>(out var stairs))
            {
                Stairs = stairs;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            /*if (other.TryGetComponent<InteractiveObject>(out var obj))
            {
                _nearObjects.Remove(obj);
            }
            else*/
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

        public void AddObject(InteractiveObject obj)
        {
            _nearObjects.Add(obj);
        }

        public void RemoveObject(InteractiveObject obj)
        {
            _nearObjects.Remove(obj);
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
            if (_nearObjects.Count == 0)
            {
                TryPutObject();
                return;
            }

            TakebleObjectContainer objContainer;

            for (int i = 0; i < _nearObjects.Count; i++)
            {
                objContainer = _nearObjects[i] as TakebleObjectContainer;

                if (objContainer != null)
                {
                    if (objContainer.CanReturn)
                    {
                        objContainer.Return();
                        TakenObject = null;
                        return;
                    }
                }
            }

            TakebleObject takebleObj;
            IReactionObject reactObj;

            for (int i = 0; i < _nearObjects.Count; i++)
            {
                reactObj = _nearObjects[i] as IReactionObject;

                if (reactObj != null)
                {
                    if (reactObj.CanReact)
                    {
                        reactObj.React();
                        return;
                    }
                }
            }

            for (int i = 0; i < _nearObjects.Count; i++)
            {
                takebleObj = _nearObjects[i] as TakebleObject;
                objContainer = _nearObjects[i] as TakebleObjectContainer;

                TryPutObject();
                if (IsHoldObject)
                {
                    return;
                }

                if (objContainer != null)
                {
                    if (TryReciveObject(objContainer))
                    {
                        return;
                    }
                }
                else if (takebleObj != null)
                {
                    if (TryTakeObject(takebleObj))
                    {
                        _nearObjects.Remove(takebleObj);
                        return;
                    }
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

        private bool TryReciveObject(TakebleObjectContainer objContainer)
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