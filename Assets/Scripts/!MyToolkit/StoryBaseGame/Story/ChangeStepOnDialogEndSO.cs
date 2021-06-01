using UnityEngine;

using Deblue.DialogSystem;
using Deblue.ObservingSystem;

namespace Deblue.Story
{
    [CreateAssetMenu(fileName = "ChangeStepCondition_DialogEnd", menuName = "Story/Change Step Conditions/Dialog End")]
    public class ChangeStepOnDialogEndSO : ChangeStepConditionSO
    {
        [SerializeField] private string _targetCharacterId;

        private IObserver _dialogEndObserver;

        protected override void MyInit()
        {
            _dialogEndObserver = DialogSwitcher.Events.SubscribeOnDialogEnd(context =>
            {
                if (context.Character.CharacterId == _targetCharacterId)
                {
                    OnDone();
                }
            });
        }

        protected override void MyDeInit()
        {
            _dialogEndObserver?.Dispose();
        }
    }
}