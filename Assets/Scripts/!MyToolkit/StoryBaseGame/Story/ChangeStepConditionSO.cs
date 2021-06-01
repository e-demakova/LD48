using UnityEngine;

namespace Deblue.Story
{
    [CreateAssetMenu(fileName = "ChangeStepCondition_Empty", menuName = "Story/Change Step Conditions/Empty")]
    public class ChangeStepConditionSO : ScriptableObject
    {
        [System.NonSerialized] public bool IsDone;

        public void Init()
        {
            MyInit();
        }

        protected virtual void MyInit()
        {
        }

        public void DeInit()
        {
            IsDone = false;
            MyDeInit();
        }

        protected virtual void MyDeInit()
        {
        }

        public virtual void CheckCondition(float deltaTime)
        {
        }

        protected void OnDone()
        {
            IsDone = true;
        }
    }
}