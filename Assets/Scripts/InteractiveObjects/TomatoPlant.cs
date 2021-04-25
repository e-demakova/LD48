using UnityEngine;

namespace Deblue.LD48
{
    public class TomatoPlant : TakebleObjectContainer
    {
        public override bool CanReturn => false;

        public override bool CanTake => _currentState == _growStates.Length - 1;

        protected override bool CanHighlight => CanTake || _player.TakenObject is WateringCan;

        [SerializeField] private SpritePair[] _growStates;
        [SerializeField] private Tomato       _tomato;

        private int _currentState;

        public void Interact()
        {
            //TODO: Сделать таймер поливания/роста.
            if(_player.TakenObject is WateringCan)
            {
                _currentState++;
                Renderer.sprite = _growStates[_currentState].Highlight;
            }
        }

        public override void Return()
        {
            throw new System.Exception("Tomato plant can't receive tomato back.");
        }

        public override TakebleObject Take()
        {
            var tomato = _tomato;
            _tomato = null;
            return tomato;
        }

        protected override void Highlight()
        {
            Renderer.sprite = _growStates[_currentState].Highlight;
        }

        protected override void StopHighlight()
        {
            Renderer.sprite = _growStates[_currentState].Standart;
        }
    }
}