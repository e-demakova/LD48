using UnityEngine;

using Deblue.DialogSystem;
using Deblue.InputSystem;
using Deblue.ObservingSystem;

namespace Deblue.LD48
{
    [RequireComponent(typeof(Collider2D))]
    public class OldMan : MonoBehaviour
    {
        public IReadonlyObservLimitProperty<int> CurrentDialog => _currentDialog;

        [SerializeField] private SpriteRenderer _avalibleDialogView;
        [SerializeField] private SpriteRenderer _keyView;
        [SerializeField] private DialogSO[]     _dialogs;
        [SerializeField] private ObservInt      _currentDialog;
        [SerializeField] private int            _unlockedDialogs;

        private Player _player;

        private void Awake()
        {
            _currentDialog = new ObservInt(0, _dialogs.Length);
            StartDialog();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out var player) && _currentDialog < _unlockedDialogs)
            {
                _player = player;
                InputReciver.SubscribeOnInput<On_Button_Down>(StartDialog, KeyCode.E);
                _keyView.enabled = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                _keyView.enabled = false;
                InputReciver.UnsubscribeOnInput<On_Button_Down>(StartDialog, KeyCode.E);
            }
        }

        public void UnlockDialog()
        {
            _unlockedDialogs++;
            _avalibleDialogView.enabled = true;
        }

        private void StartDialog(On_Button_Down context)
        {
            StartDialog();
        }

        private void StartDialog()
        {
            if (_currentDialog < _unlockedDialogs && !DialogSwitcher.IsInDialg)
            {
                _keyView.enabled = false;
                //DialogSwitcher.StartDialog(_dialogs[(int)_currentDialog]);
                if (_currentDialog == 5)
                {
                    var cup = _player.TakeObject();
                    cup.gameObject.SetActive(false);
                    cup.transform.SetParent(transform);
                }
                _currentDialog++;
                if (_currentDialog >= _unlockedDialogs)
                {
                    _avalibleDialogView.enabled = false;
                }
            }
        }
    }
}