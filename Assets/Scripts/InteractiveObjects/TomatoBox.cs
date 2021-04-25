using UnityEngine;

namespace Deblue.LD48
{
    public class TomatoBox : TakebleObjectContainer
    {
        public override bool CanReturn => _player.TakenObject is Tomato && _tomatoesCount < _tomatoes.Length;

        public override bool CanTake => _tomatoes.Length > 0; 

        protected override bool CanHighlight => _player.TakenObject is Tomato;

        protected Tomato[] _tomatoes = new Tomato[4];
        protected int _tomatoesCount;

        public override ITakebleObject Take(Vector3 takePosition)
        {
            _tomatoesCount--;
            var tomato = _tomatoes[_tomatoesCount];
            _tomatoes[_tomatoesCount] = null;

            return tomato;
        }

        public override void Return()
        {
            _tomatoes[_tomatoesCount] = (Tomato)_player.TakenObject;
            _tomatoesCount++;
        }

        protected override void Highlight()
        {
            throw new System.NotImplementedException();
        }

        protected override void StopHighlight()
        {
            throw new System.NotImplementedException();
        }
    }
}
