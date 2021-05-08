using UnityEngine;

using Deblue.ObservingSystem;
using Deblue.Story;

namespace Deblue.LD48
{
    [CreateAssetMenu(fileName = "ChangeStepCondition_PlayerOutside", menuName = "Story/Change Step Conditions/Player Outside")]
    public class ChangeStepOnPlayerOutside : ChangeStepConditionSO
    {
        private IObserver _triggerObserver;

        protected override void MyInit()
        {
            var triggers = FindObjectOfType<Triggers>();
            _triggerObserver = Triggers.PlayerOutside.Subscribe(context => OnDone());
        }

        protected override void MyDeInit()
        {
            _triggerObserver.Dispose();
        }
    }
}