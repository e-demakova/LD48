using UnityEngine;
using UnityEngine.UI;

using Deblue.ObservingSystem;
using Deblue.Interactive;

namespace Deblue.LD48
{
    public class TomatoPlant : TakebleObjectContainer, IReactionObject
    {
        public IReadonlyObservProperty<bool> IsGrow => _isGrow;

        [SerializeField] private string        _wateringCanId;
        [SerializeField] private SpritePair[]  _growStates;
        [SerializeField] private TakebleObject _tomato;
        [SerializeField] private Slider        _slider;

        private int _currentState;

        private ObservFloat _timer = new ObservFloat(0, 1f);
        private ObservBool  _isGrow = new ObservBool();

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
            if (_isGrow)
            {
                _timer += Time.fixedDeltaTime;
            };
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

        public bool CanReact(IObjectTaker taker)
        {
            return taker.IsContainObject(_wateringCanId) &&
                   _currentState < _growStates.Length - 2 &&
                   !_isGrow;
        }

        public void React(IObjectTaker taker)
        {
            StartTimer();
            _updated.Raise(new Interact_Object_Updated(this));
        }

        private void StartTimer()
        {
            _timer.Value = 0f;
            _isGrow.Value = true;
            ShowSlider();
        }

        private void StopTimer(Property_Reached_Upper_Limit context)
        {
            _isGrow.Value = false;
            HideSlider();

            _currentState++;
            _updated.Raise(new Interact_Object_Updated(this));
        }

        public override void Return(TakebleObject obj)
        {
            throw new System.Exception("Tomato plant can't receive tomato back.");
        }

        public override bool CanTake(IObjectTaker taker)
        {
            return _currentState == _growStates.Length - 2;
        }

        public override TakebleObject Take()
        {
            _tomato.gameObject.SetActive(true);
            var tomato = _tomato;

            _tomato = null;
            _currentState++;

            return tomato;
        }

        public override bool CanHighlight(IObjectTaker taker)
        {
            return CanTake(taker) || CanReact(taker);
        }

        public override void Highlight()
        {
            Renderer.sprite = _growStates[_currentState].Highlight;
        }

        public override void StopHighlight()
        {
            Renderer.sprite = _growStates[_currentState].Standart;
            _keyView.enabled = false;
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

        public override bool CanReturn(string objId)
        {
            return false;
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