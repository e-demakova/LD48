using UnityEngine;

using Deblue.ObservingSystem;

namespace Deblue.Stats
{
    public abstract class BaseStatView : MonoBehaviour
    {
        public abstract void Init(IReadonlyObservLimitProperty<float> property);
        public abstract void DeInit();
        public abstract void UpdateView(Limited_Property_Changed<float> context);
    }

    public abstract class StatView<TEnum> : BaseStatView where TEnum : System.Enum
    {
        [SerializeField] private TEnum _statId;
        public TEnum StatId => _statId;

        protected IReadonlyObservLimitProperty<float> _property;

        public sealed override void Init(IReadonlyObservLimitProperty<float> property)
        {
            _property = property;
            _property.SubscribeOnChanging(UpdateView);
        }

        protected virtual void Init()
        {
        }

        public sealed override void DeInit()
        {
            _property.UnsubscribeOnChanging(UpdateView);
        }
    }
}