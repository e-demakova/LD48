using UnityEngine;
using UnityEngine.UI;

using Deblue.ObservingSystem;
using Deblue.Interactive;

namespace Deblue.LD48
{
    public class TomatoPlant : TakebleObjectContainer, IReactionObject
    {
        public bool CanReact => Player.TakenObject is WateringCan &&
                                _currentState < _growStates.Length - 2 &&
                                !_isGrow;
        public override bool CanReturn => false;
        public override bool CanTake => _currentState == _growStates.Length - 2;
        public IReadonlyObservProperty<bool> IsGrow => _isGrow;
        protected override bool CanHighlight => CanTake || CanReact;

        [SerializeField] private SpritePair[] _growStates;
        [SerializeField] private Tomato       _tomato;
        [SerializeField] private Slider       _slider;

        private int _currentState;

        private ObservFloat _timer = new ObservFloat(0, 40f);
        private ObservBool  _isGrow;

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

        private void FixedUpdate()
        {
            if (_isGrow)
            {
                _timer += Time.fixedDeltaTime;
            };
        }

        public void React()
        {
            if (Player.TakenObject is WateringCan)
            {
                StartTimer();
            }
        }

        private void StartTimer()
        {
            _timer.Value = 0f;
            _isGrow.Value = true;
            ShowSlider();
            TryHilight();
        }

        private void StopTimer(Property_Reached_Upper_Limit context)
        {
            _isGrow.Value = false;
            HideSlider();

            _currentState++;
            TryHilight();
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
            _keyView.enabled = false;
        }
    }

    public readonly struct Tomato_Start_Grow
    {
        public readonly int State;

        public Tomato_Start_Grow(int state)
        {
            State = state;
        }
    }
}