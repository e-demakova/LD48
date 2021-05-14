using UnityEngine;

using Deblue.Story;
using Deblue.ObservingSystem;

namespace Deblue.LD48
{
    public enum CompareType
    {
        Equal,
        More,
        Less
    }

    [CreateAssetMenu(fileName = "ChangeStepCondition_TomatosInBox", menuName = "Story/Change Step Conditions/Tomatos In Box")]
    public class ChangeStepOnTomatosInBoxSO : ChangeStepConditionSO
    {
        [SerializeField] private int         _targetTomatosCount;
        [SerializeField] private CompareType _compareType;

        private IObserver _tomatoesObserver;

        protected override void MyInit()
        {
            var box = FindObjectOfType<TomatoBox>();

            CompareCount(box.TomatoesCount.Value);

            if (!IsDone)
            {
                box.TomatoesCount.SubscribeOnChanging(context => CompareCount(context.NewValue));
            }
        }

        protected override void MyDeInit()
        {
            _tomatoesObserver?.Dispose();
        }

        private void CompareCount(int tomatoes)
        {
            var isDone = false;

            switch (_compareType)
            {
                case CompareType.Equal:
                    isDone = tomatoes == _targetTomatosCount;
                    break;

                case CompareType.More:
                    isDone = tomatoes > _targetTomatosCount;
                    break;

                case CompareType.Less:
                    isDone = tomatoes < _targetTomatosCount;
                    break;
            }

            if (isDone)
            {
                OnDone();
            }
        }
    }
}