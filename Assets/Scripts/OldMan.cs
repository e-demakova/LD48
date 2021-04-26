using System;

using UnityEngine;

using Deblue.DialogSystem;
using Deblue.InputSystem;

namespace Deblue.LD48
{
    [RequireComponent(typeof(Collider2D))]
    public class OldMan : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _avalibleDialogView;
        [SerializeField] private SpriteRenderer _keyView;
        [SerializeField] private DialogSO[]     _dialogs;
        [SerializeField] private int            _unlockedDialogs;
        [SerializeField] private int            _currentDialog;

        private Type   _requiredObject;
        private Player _player;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out var player))
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
            if (_player.TakenObject != null)
            {
                if (_player.TakenObject.GetType() == _requiredObject)
                {
                    _unlockedDialogs++;
                }
            }
            if (_currentDialog < _unlockedDialogs)
            {
                DialogSwitcher.StartDialog(_dialogs[_currentDialog]);
                _currentDialog++;
                _avalibleDialogView.enabled = false;
            }
        }
    }
}