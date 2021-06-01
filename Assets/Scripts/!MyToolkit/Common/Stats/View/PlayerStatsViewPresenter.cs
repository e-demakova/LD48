using System.Collections.Generic;

using UnityEngine;

using Deblue.ObservingSystem;
using Deblue.UiViews;

namespace Deblue.Stats
{
    public class PlayerStatsViewPresenter<TEnum> : ModelView<StatsStorage<TEnum>> where TEnum : System.Enum
    {
        [SerializeField] private StatView<TEnum>[] _views;

        private Dictionary<TEnum, StatView<TEnum>> _statsViews = new Dictionary<TEnum, StatView<TEnum>>(5);

        protected override void MyInit()
        {
            _model.StatsIds.SubscribeOnAdding(InitStatView);
            _model.StatsIds.SubscribeOnRemoving(DestroyStatView);
        }

        public override void DeInit()
        {
            _model.StatsIds.UnsubscribeOnAdding(InitStatView);
            _model.StatsIds.UnsubscribeOnRemoving(DestroyStatView);
        }

        private void InitStatView(Value_Added<int, TEnum> context)
        {
            if (_statsViews.ContainsKey(context.Value))
            {
                return;
            }
            for (int i = 0; i < _views.Length; i++)
            {
                if (_views[i].StatId.Equals(context.Value))
                {
                    _statsViews.Add(context.Value, _views[i]);
                    var stat = _model.GetStat(context.Value);
                    _views[i].Init(stat);
                    break;
                }
            }
        }

        private void DestroyStatView(Value_Removed<int, TEnum> context)
        {
            if (_statsViews.TryGetValue(context.Value, out var view))
            {
                view.DeInit();
                Destroy(view.gameObject);
            }
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }
    }
}
