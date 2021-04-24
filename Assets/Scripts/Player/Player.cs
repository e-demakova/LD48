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
        public InteractiveObject TakenObject { get; private set; }
        public Stairs Stairs { get; private set; }
        public float Speed => _speed;
        public bool IsHoldObject => TakenObject != null;
        public bool IsTalk { get; private set; }

        [SerializeField] private float _speed;

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


            InputReciver.SubscribeOnInput<On_Button_Down>(TryTakeOrPutObject, KeyCode.F);
        }

        private void OnDisable()
        {
            DialogSwitcher.Events.UnsubscribeOnDialogStart(OnDialogStart);
            DialogSwitcher.Events.UnsubscribeOnDialogEnd(OnDialogEnd);

            InputReciver.UnsubscribeOnInput<On_Button_Down>(TryTakeOrPutObject, KeyCode.F);
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
                obj.OnPlayerEnter(this);
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
                _nearObject?.OnPlayerExit();
                _nearObject = null;
            }
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

        private void TryTakeOrPutObject(On_Button_Down context)
        {
            TryPutObject();
            if (_nearObject == null || IsHoldObject)
            {
                return;
            }
            if (_nearObject.CanTake())
            {
                _nearObject.Take();
                TakenObject = _nearObject;
                _nearObject = null;
                    Debug.Log("Take");
            }
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
                if (TakenObject.CanPut())
                {
                    TakenObject.Put();
                    TakenObject = null;
                    Debug.Log("Put");
                    return true;
                }
            }
            return false;
        }
    }
}