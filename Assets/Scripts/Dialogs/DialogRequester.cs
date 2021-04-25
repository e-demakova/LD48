using UnityEngine;

using Deblue.DialogSystem;
using Deblue.InputSystem;

namespace Deblue.LD48
{
    [RequireComponent(typeof(Collider2D))]
    public class DialogRequester : MonoBehaviour
    {
        [SerializeField] private DialogSO[] _dialogs;
        [SerializeField] private int        _unlockedDialogs;
        [SerializeField] private int        _currentDialog;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                InputReciver.SubscribeOnInput<On_Button_Down>(StartDialog, KeyCode.E);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                InputReciver.UnsubscribeOnInput<On_Button_Down>(StartDialog, KeyCode.E);
            }
        }

        public void UnlockDialog()
        {
            _unlockedDialogs++;
        }

        private void StartDialog(On_Button_Down context)
        {
            if (_currentDialog != _unlockedDialogs)
            {
                DialogSwitcher.StartDialog(_dialogs[_currentDialog]);
                _currentDialog++;
            }
        }
    }
}