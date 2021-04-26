using UnityEngine;
using UnityEngine.UI;

using Deblue.ObservingSystem;

namespace Deblue.LD48
{
    public class TeaStation : TakebleObjectContainer, IReactionObject
    {
        public bool CanReact => !_teaReady && !_timerRun && !_cupTaken && IsAvalible;
        public override bool CanReturn => _cupTaken && _player.TakenObject is CupOfTea;
        public override bool CanTake => !_cupTaken && _teaReady;
        protected override bool CanHighlight => CanTake || CanReturn || CanReact;

        public Handler<Cup_Taken> CupTaken = new Handler<Cup_Taken>();
        public bool IsAvalible;

        [SerializeField] private SpritePair _withTea;
        [SerializeField] private SpritePair _withoutTea;
        [SerializeField] private CupOfTea   _cup;
        [SerializeField] private Slider     _slider;

        private bool _cupTaken;

        public override TakebleObject Take()
        {
            _cupTaken = true;
            _cup.gameObject.SetActive(true);
            Renderer.sprite = _withoutTea.Highlight;
            CupTaken.Raise(new Cup_Taken());
            return _cup;
        }

        private ObservFloat _timer = new ObservFloat(0, 9f);
        private bool        _timerRun;
        private bool        _teaReady;

        protected override void MyAwake()
        {
            _timer.SubscribeOnUpperLimit(StopTimer);
            _timer.SubscribeOnChanging(ChangeSliderValue);

            _slider.maxValue = _timer.UpperLimit;
            _slider.minValue = _timer.LowerLimit;
            _slider.value = _timer.Value;
        }

        private void OnDestroy()
        {
            _timer.UnsubscribeOnUpperLimit(StopTimer);
            _timer.UnsubscribeOnChanging(ChangeSliderValue);
        }

        private void ChangeSliderValue(Property_Changed<float> context)
        {
            _slider.value = context.NewValue;
        }

        private void ShowSlider()
        {
            _slider.gameObject.SetActive(true);
        }

        private void HideSlider()
        {
            _slider.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (_timerRun)
            {
                _timer += Time.fixedDeltaTime;
            }
            TryHilight();
        }

        private void StartTimer()
        {
            _timer.Value = 0f;
            _timerRun = true;
            ShowSlider();
        }

        private void StopTimer(Property_Reached_Upper_Limit context)
        {
            _timerRun = false;
            _teaReady = true;
            HideSlider();
        }

        public override void Return()
        {
            _cupTaken = false;
            _cup.transform.SetParent(transform);
            _cup.gameObject.SetActive(false);
            Renderer.sprite = _withTea.Highlight;
        }

        protected override void Highlight()
        {
            Renderer.sprite = _cupTaken ? _withoutTea.Highlight : _withTea.Highlight;
        }

        protected override void StopHighlight()
        {
            Renderer.sprite = _cupTaken ? _withoutTea.Standart : _withTea.Standart;
            _keyView.enabled = false;
        }

        public void React()
        {
            StartTimer();
        }
    }

    public readonly struct Cup_Taken { }
}