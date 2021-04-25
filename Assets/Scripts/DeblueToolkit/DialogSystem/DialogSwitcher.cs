using System;

using Deblue.ObservingSystem;

namespace Deblue.DialogSystem
{
    public class DialogSwitcher : UniqMono<DialogSwitcher>
    {
        public static IDialogSwitcherEvents Events => _events;

        private static DialogSwitcherEvents _events = new DialogSwitcherEvents();

        private static DialogSO _currentDialog;

        public static void StartDialog(DialogSO dialog)
        {
            _currentDialog = dialog;
            _events.Raise(new Dialog_Start(dialog));
            _currentDialog.Init();
            SwitchReplica();
        }

        public static void CloseCurrentDialog()
        {
            _events.Raise(new Dialog_End(_currentDialog));
            _currentDialog = null;
        }

        public static bool TryCloseDialog(DialogSO dialog)
        {
            if (dialog == _currentDialog)
            {
                CloseCurrentDialog();
                return true;
            }
            return false;
        }

        public void SwithReplicaOnButton()
        {
            SwitchReplica();
        }

        public static void SwitchReplica()
        {
            if (_currentDialog.TrySwitchToNextReplica(out var replica))
            {
                _events.Raise(new Replica_Switch(replica));
            }
            else
            {
                CloseCurrentDialog();
            }
        }
    }

    public interface IDialogSwitcherEvents
    {
        void SubscribeOnDialoguesOver(Action<Dialogues_Over> action);
        void SubscribeOnDialogStart(Action<Dialog_Start> action);
        void SubscribeOnDialogEnd(Action<Dialog_End> action);
        void SubscribeOnReplicaSwitch(Action<Replica_Switch> action);

        void UnsubscribeOnDialoguesOver(Action<Dialogues_Over> action);
        void UnsubscribeOnDialogStart(Action<Dialog_Start> action);
        void UnsubscribeOnDialogEnd(Action<Dialog_End> action);
        void UnsubscribeOnReplicaSwitch(Action<Replica_Switch> action);
    }

    public class DialogSwitcherEvents : EventSender, IDialogSwitcherEvents
    {
        #region Subscribing
        public void SubscribeOnDialoguesOver(Action<Dialogues_Over> action)
        {
            Subscribe(action);
        }

        public void SubscribeOnDialogStart(Action<Dialog_Start> action)
        {
            Subscribe(action);
        }

        public void SubscribeOnDialogEnd(Action<Dialog_End> action)
        {
            Subscribe(action);
        }

        public void SubscribeOnReplicaSwitch(Action<Replica_Switch> action)
        {
            Subscribe(action);
        }
        #endregion

        #region Unsubscribing
        public void UnsubscribeOnDialoguesOver(Action<Dialogues_Over> action)
        {
            Unsubscribe(action);
        }

        public void UnsubscribeOnDialogStart(Action<Dialog_Start> action)
        {
            Unsubscribe(action);
        }

        public void UnsubscribeOnDialogEnd(Action<Dialog_End> action)
        {
            Unsubscribe(action);
        }

        public void UnsubscribeOnReplicaSwitch(Action<Replica_Switch> action)
        {
            Unsubscribe(action);
        }
        #endregion
    }
}
