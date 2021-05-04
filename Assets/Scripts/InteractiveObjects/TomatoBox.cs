using UnityEngine;

using Deblue.ObservingSystem;
using Deblue.Interactive;

namespace Deblue.LD48
{
    public class TomatoBox : TakebleObjectContainer
    {
        protected Tomato[]  _tomatoes = new Tomato[4];
        protected ObservInt _tomatoesCount;

        [SerializeField] private SpritePair[] _sprites;

        public IReadonlyObservLimitProperty<int> TomatoesCount => _tomatoesCount;
        public override bool CanReturn => Player.TakenObject is Tomato && _tomatoesCount < _tomatoes.Length;
        public override bool CanTake => _tomatoesCount > 0;
        protected override bool CanHighlight => CanReturn || CanTake;

        public override TakebleObject Take()
        {
            _tomatoesCount--;
            var tomato = _tomatoes[(int)_tomatoesCount];
            tomato.gameObject.SetActive(true);
            _tomatoes[(int)_tomatoesCount] = null;
            Renderer.sprite = _sprites[(int)_tomatoesCount].Highlight;
            return tomato;
        }

        public override void Return()
        {
            var tomato = (Tomato)Player.TakenObject;
            tomato.transform.SetParent(transform);
            tomato.gameObject.SetActive(false);
            _tomatoes[(int)_tomatoesCount] = tomato;
            _tomatoesCount++;
            Renderer.sprite = _sprites[(int)_tomatoesCount].Highlight;
        }

        protected override void Highlight()
        {
            Renderer.sprite = _sprites[(int)_tomatoesCount].Highlight;
        }

        protected override void StopHighlight()
        {
            Renderer.sprite = _sprites[(int)_tomatoesCount].Standart;
        }
    }

    public struct Box_Full
    {
    }
}
