using UnityEngine;

using Deblue.ObservingSystem;
using Deblue.Story;

namespace Deblue.LD48
{
    [CreateAssetMenu(fileName = "ChangeStepCondition_PlayerOnDoor", menuName = "Story/Change Step Conditions/Player On Door")]
    public class ChangeStepOnPlayerOnDoor : ChangeStepConditionSO
    {
        private IObserver _triggerObserver;

        protected override void MyInit()
        {
            var triggers = FindObjectOfType<Triggers>();
            _triggerObserver = Triggers.PlayerOnDoor.Subscribe(context => OnDone());
        }

        protected override void MyDeInit()
        {
            _triggerObserver.Dispose();
        }
    }
}