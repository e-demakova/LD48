using System.Collections;
using UnityEngine;

namespace Deblue.LD48
{
    public class DippingVisualizator : UniqMono<DippingVisualizator>
    {
        [SerializeField] private CameraShaker _shaker;

        [SerializeField] private GameObject  _endScreen;
        [SerializeField] private AudioSource _loudEarth;
        [SerializeField] private Animator    _background;
        [SerializeField] private Animator    _midground;
        [SerializeField] private Animator    _frontground;
        [SerializeField] private Animator    _bunker;
        [SerializeField] private Animator[]  _rayes;

        private static int _dipperTrigget = Animator.StringToHash("Deeper");
        private static int _blinkTrigget = Animator.StringToHash("Blink");
        private static int _blinkLongTrigget = Animator.StringToHash("LongBlink");
        private static int _endTrigget = Animator.StringToHash("End");
        private static int _isActive = Animator.StringToHash("IsActive");

        private float _timeToNextShake;
        private float _lastShakeTime;
        private bool  _isOver;

        private Coroutine _activeRay;

        private void OnEnable()
        {
            LD48GameModel.Events.SubscribeOnGameStateChange(VisualizeNewState);
        }

        private void OnDisable()
        {
            LD48GameModel.Events.UnsubscribeOnGameStateChange(VisualizeNewState);
        }

        private void Update()
        {
            if (Time.realtimeSinceStartup >= _timeToNextShake + _lastShakeTime && !_isOver)
            {
                ShowRay();
                _timeToNextShake = Random.Range(1f, 1f);
                _lastShakeTime = Time.realtimeSinceStartup;

                _shaker.ShakingTime = 0.7f;
                //_shaker.Intensity = 0.5f;

                _shaker.StartShake();
                StartBlink();
            }
        }

        private void VisualizeNewState(Game_State_Change context)
        {
            ShowRay();
            _lastShakeTime = Time.realtimeSinceStartup;
            _shaker.ShakingTime = 2f;
            _shaker.Intensity = 2f;
            _shaker.StartShake();
            _loudEarth.Play();
            _background.SetTrigger(_dipperTrigget);
            _midground.SetTrigger(_dipperTrigget);
            _frontground.SetTrigger(_dipperTrigget);

            if (context.NewState == GameState.Over)
            {
                StartLastBlink();
                _endScreen.SetActive(true);
                _isOver = true;
            }
            else
            {
                StartBlink();
            }
        }

        private void ShowRay()
        {
            if (_activeRay == null)
            {
                _activeRay = StartCoroutine(ShowingRay());
            }
        }

        private IEnumerator ShowingRay()
        {
            int index1, index2, index3;

            index1 = index2 = index3 = Random.Range(0, _rayes.Length);
            _rayes[index1].SetBool(_isActive, true);

            if (Random.value < 0.5f)
            {
                while (index1 == index2)
                {
                    index2 = Random.Range(0, _rayes.Length);
                }
                _rayes[index2].SetBool(_isActive, true);
            }
            if (Random.value < 0.2f)
            {
                while (index3 == index2 || index3 == index1)
                {
                    index3 = Random.Range(0, _rayes.Length);
                }
                _rayes[index3].SetBool(_isActive, true);
            }

            var startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < startTime + 1f)
            {
                yield return new WaitForFixedUpdate();
            }
            _rayes[index1].SetBool(_isActive, false);
            _rayes[index2].SetBool(_isActive, false);
            _rayes[index3].SetBool(_isActive, false);
            _activeRay = null;
        }

        private void StartBlink()
        {
            //_bunker.SetTrigger(Random.value < 0.5f ? _blinkTrigget : _blinkLongTrigget);
        }

        private void StartLastBlink()
        {
            //_bunker.SetTrigger(_endTrigget);
        }
    }
}