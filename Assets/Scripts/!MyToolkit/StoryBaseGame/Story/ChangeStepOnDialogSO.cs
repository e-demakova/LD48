using UnityEngine;

using Deblue.DialogSystem;

namespace Deblue.Story
{
    [CreateAssetMenu(fileName = "ChangeStepCondition_StartDialog", menuName = "Story/Change Step Conditions/Start Dialog")]
    public class ChangeStepOnDialogSO : ChangeStepConditionSO
    {
        [SerializeField] private string _targetCharacterId;

        protected override void MyInit()
        {
            DialogSwitcher.Events.SubscribeOnDialogStart(OnDone);
        }

        protected override void MyDeInit()
        {
            DialogSwitcher.Events.UnsubscribeOnDialogStart(OnDone);
        }

        private void OnDone(Dialog_Start context)
        {
            if (context.Character.CharacterId == _targetCharacterId)
            {
                OnDone();
            }
        }
    }
}