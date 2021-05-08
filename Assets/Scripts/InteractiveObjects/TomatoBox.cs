using UnityEngine;

using Deblue.ObservingSystem;
using Deblue.Interactive;

namespace Deblue.LD48
{
    public class TomatoBox : TakebleObjectContainer
    {
        protected TakebleObject[]  _tomatoes = new StandartTakebleObject[4];
        protected ObservInt _tomatoesCount = new ObservInt();

        [SerializeField] private SpritePair[] _sprites;

        public IReadonlyObservLimitProperty<int> TomatoesCount => _tomatoesCount;
        public override bool CanTake(IObjectTaker taker) 
        {
            return _tomatoesCount > 0; 
        }

        public override TakebleObject Take()
        {
            _tomatoesCount--;

            var tomato = _tomatoes[_tomatoesCount];
            tomato.gameObject.SetActive(true);

            _tomatoes[_tomatoesCount] = null;

            return tomato;
        }

        public override bool CanReturn(string objId) 
        { 
            return _conteinedObjectId == objId && _tomatoesCount < _tomatoes.Length; 
        }

        public override void Return(TakebleObject tomato)
        {
            tomato.transform.SetParent(transform);
            tomato.gameObject.SetActive(false);

            _tomatoes[_tomatoesCount] = tomato;
            _tomatoesCount++;

            Renderer.sprite = _sprites[_tomatoesCount].Highlight;
        }

        public override bool CanHighlight(IObjectTaker taker)
        {
            return CanReturn(taker.TakenObject) || CanTake(taker);
        }


        public override void Highlight()
        {
            Renderer.sprite = _sprites[_tomatoesCount].Highlight;
        }

        public override void StopHighlight()
        {
            Renderer.sprite = _sprites[_tomatoesCount].Standart;
        }
    }
}
