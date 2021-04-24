using System;
using System.Collections.Generic;

using Deblue.ObservingSystem;
using Deblue.Models;

namespace Deblue.Stats
{
    public interface IStatsStorage
    {

    }

    public class StatsStorage<TEnum> : IModel where TEnum : Enum
    {
        public IReadonlyObservList<TEnum> StatsIds => _statsIds;

        private ObservList<TEnum> _statsIds = new ObservList<TEnum>(5);

        private Dictionary<TEnum, ObservFloat> _statsAmount =
            new Dictionary<TEnum, ObservFloat>(5);

        public IReadonlyObservLimitProperty<float> GetStat(TEnum id)
        {
            if (_statsAmount.TryGetValue(id, out var stat))
            {
                return stat;
            }
            throw new ArgumentException($"Try getting unregistered stat {id}.");
        }

        public void AddStat(TEnum id, float amount = 0)
        {
            AddStat(id, amount, float.MinValue);
        }
        
        public void AddStat(TEnum id, float amount, float lowerLimit, float upperLimit = float.MaxValue)
        {
            if (!_statsAmount.TryGetValue(id, out var stat))
            {
                stat = new ObservFloat(amount, lowerLimit, upperLimit);
                _statsAmount.Add(id, stat);
                _statsIds.Add(id);
            }
#if UNITY_EDITOR
            else
            {
                UnityEngine.Debug.LogWarningFormat($"Try adding already existing stat {id}.");
            }
#endif
        }

        public void ChangeAmount(TEnum id, float delta)
        {
            if (_statsAmount.TryGetValue(id, out var stat))
            {
                stat += delta;
            }
        }

        public void SetAmount(TEnum id, float amount)
        {
            if (_statsAmount.TryGetValue(id, out var stat))
            {
                stat = amount;
            }
        }

        public void DeInit()
        {
            _statsIds.Clear();
            foreach (var amount in _statsAmount)
            {
                amount.Value.Dispose();
            }
        }
    }
}