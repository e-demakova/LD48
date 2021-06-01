using UnityEngine;

using Deblue.DialogSystem;

namespace Deblue.Story
{
    [CreateAssetMenu(fileName = "ChangeStepCondition_GiveItem", menuName = "Story/Change Step Conditions/Give Item")]
    public class ChangeStepOnGiveItemSO : ChangeStepConditionSO
    {
        [SerializeField] private string _targetCharacterId;
        [SerializeField] private string _itemId;

        protected override void MyInit()
        {
            DialogSwitcher.Events.SubscribeOnPlayerGiveItem(CheckGivenItem);
        }

        protected override void MyDeInit()
        {
            DialogSwitcher.Events.UnsubscribeOnPlayerGiveItem(CheckGivenItem);
        }

        private void CheckGivenItem(Player_Give_Item context)
        {
            if (context.Character.CharacterId == _targetCharacterId && context.ItemId == _itemId)
            {
                OnDone();
            }
        }
    }
}