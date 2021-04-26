﻿using UnityEngine;

using Deblue.ObservingSystem;

namespace Deblue.LD48
{
    public class TomatoPlant : TakebleObjectContainer, IReactionObject
    {
        public bool CanReact => _player.TakenObject is WateringCan && _currentState < _growStates.Length - 2;
        public override bool CanReturn => false;
        public override bool CanTake => _currentState == _growStates.Length - 2;
        protected override bool CanHighlight => CanTake || CanReact;

        public Handler<Tomato_Grown> TomatoGrown = new Handler<Tomato_Grown>();

        [SerializeField] private SpritePair[] _growStates;
        [SerializeField] private Tomato       _tomato;

        private int _currentState;

        public void React()
        {
            //TODO: Сделать таймер поливания/роста.
            if(_player.TakenObject is WateringCan)
            {
                _currentState++;
                TomatoGrown.Raise(new Tomato_Grown(_currentState));
                Renderer.sprite = _growStates[_currentState].Highlight;
            }
        }

        public override void Return()
        {
            throw new System.Exception("Tomato plant can't receive tomato back.");
        }

        public override TakebleObject Take()
        {
            _tomato.gameObject.SetActive(true);
            var tomato = _tomato;
            _tomato = null;
            _currentState++;
            Renderer.sprite = _growStates[_currentState].Highlight;
            _keyView.enabled = false;
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

    public readonly struct Tomato_Grown
    {
        public readonly int State;

        public Tomato_Grown(int state)
        {
            State = state;
        }
    }
}