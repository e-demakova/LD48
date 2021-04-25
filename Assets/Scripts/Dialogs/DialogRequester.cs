using UnityEngine;

using Deblue.DialogSystem;
using Deblue.InputSystem;

namespace Deblue.LD48
{
    public class DialogRequester : MonoBehaviour
    {
        [SerializeField] private int      _uniqDialogsCount;
        [SerializeField] private DialogSO _defoultDialog;

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

        private void StartDialog(On_Button_Down context)
        {

        }

        protected void RequestDialog(DialogSO dialog)
        {
            DialogSwitcher.StartDialog(dialog);
        }
    }
}
