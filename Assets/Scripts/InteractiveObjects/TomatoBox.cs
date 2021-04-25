﻿using UnityEngine;

namespace Deblue.LD48
{
    public class TomatoBox : TakebleObjectContainer
    {
        public override bool CanReturn => _player.TakenObject is Tomato && _tomatoesCount < _tomatoes.Length;
        public override bool CanTake => _tomatoesCount > 0;
        protected override bool CanHighlight => _player.TakenObject is Tomato;

        [SerializeField] private SpritePair[] _sprites;

        protected Tomato[] _tomatoes = new Tomato[4];
        protected int      _tomatoesCount;

        public override TakebleObject Take()
        {
            _tomatoesCount--;
            var tomato = _tomatoes[_tomatoesCount];
            tomato.gameObject.SetActive(true);
            _tomatoes[_tomatoesCount] = null;
            Renderer.sprite = _sprites[_tomatoesCount].Highlight;
            return tomato;
        }

        public override void Return()
        {
            var tomato = (Tomato)_player.TakenObject;
            tomato.transform.SetParent(transform);
            tomato.gameObject.SetActive(false);
            _tomatoes[_tomatoesCount] = tomato;
            _tomatoesCount++;
            Renderer.sprite = _sprites[_tomatoesCount].Highlight;
        }

        protected override void Highlight()
        {
            Renderer.sprite = _sprites[_tomatoesCount].Highlight;
        }

        protected override void StopHighlight()
        {
            Renderer.sprite = _sprites[_tomatoesCount].Standart;
        }
    }
}
