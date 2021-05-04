using UnityEngine;

using Deblue.Story;
using Deblue.ObservingSystem;
using System.Collections.Generic;

namespace Deblue.LD48
{
    [CreateAssetMenu(fileName = "ChangeStepCondition_PlantsGrow", menuName = "Story/Change Step Conditions/Plants Grow")]
    public class ChangeStepOnPlantsGrow : ChangeStepConditionSO
    {
        [SerializeField] private int  _targetPlantsCount;
        [SerializeField] private bool _targetBool;

        private int             _plantsCount;
        private List<IObserver> _observingPlants;

        protected override void MyInit()
        {
            var plants = FindObjectsOfType<TomatoPlant>();

            for (int i = 0; i < plants.Length; i++)
            {
                plants[i].IsGrow.SubscribeOnChanging(CheckIsGrow, _observingPlants);
            }
        }

        private void CheckIsGrow(Property_Changed<bool> context)
        {
            CheckIsGrow(context.NewValue);
        }

        private void CheckIsGrow(bool isGrow)
        {
            if (isGrow == _targetBool)
            {
                _plantsCount++;
                if (_plantsCount == _targetPlantsCount)
                {
                    OnDone();
                }
            }
        }

        protected override void MyDeInit()
        {
            for (int i = 0; i < _observingPlants.Count; i++)
            {
                _observingPlants[i].Dispose();
            }
            _observingPlants.Clear();
        }
    }
}
