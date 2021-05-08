using UnityEngine;
using UnityEngine.UI;

using Deblue.ObservingSystem;
using Deblue.Interactive;

namespace Deblue.LD48
{
    public class TeaStation : TakebleObjectContainer, IReactionObject
    {
        public bool CanReact(IObjectTaker taker)
        {
            return !_teaReady && !_timerRun && !_cupTaken && IsAvalible;
        }

        public bool IsAvalible;

        [SerializeField] private SpritePair    _withTea;
        [SerializeField] private SpritePair    _withoutTea;
        [SerializeField] private TakebleObject _cup;
        [SerializeField] private Slider        _slider;

        private bool _cupTaken;

        private ObservFloat _timer = new ObservFloat(0, 9f);
        private bool        _timerRun;
        private bool        _teaReady;

        public override bool CanTake(IObjectTaker taker)
        {
            return !_cupTaken && _teaReady;
        }

        public override TakebleObject Take()
        {
            _cupTaken = true;
            _cup.gameObject.SetActive(true);
            Renderer.sprite = _withoutTea.Highlight;
            return _cup;
        }

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

        private void FixedUpdate()
        {
            if (_timerRun)
            {
                _timer += Time.fixedDeltaTime;
            }
        }

        private void ChangeSliderValue(Limited_Property_Changed<float> context)
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

        public override bool CanReturn(string objId)
        {
            return _cupTaken && objId == _conteinedObjectId;
        }

        public override void Return(TakebleObject obj)
        {
            _cupTaken = false;
            _cup.transform.SetParent(transform);
            _cup.gameObject.SetActive(false);
            Renderer.sprite = _withTea.Highlight;
        }

        public override bool CanHighlight(IObjectTaker taker)
        {
            return CanTake(taker) || CanReturn(taker.TakenObject) || CanReact(taker);
        }

        public override void Highlight()
        {
            Renderer.sprite = _cupTaken ? _withoutTea.Highlight : _withTea.Highlight;
        }

        public override void StopHighlight()
        {
            Renderer.sprite = _cupTaken ? _withoutTea.Standart : _withTea.Standart;
            _keyView.enabled = false;
        }

        public void React(IObjectTaker taker)
        {
            StartTimer();
        }

        public bool TryReact(IObjectTaker taker)
        {
            if (CanReact(taker))
            {
                React(taker);
                return true;
            }
            return false;
        }
    }
}