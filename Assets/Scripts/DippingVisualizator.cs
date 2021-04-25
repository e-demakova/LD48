using UnityEngine;

namespace Deblue.LD48
{
    public class DippingVisualizator : UniqMono<DippingVisualizator>
    {
        [SerializeField] private CameraShaker _shaker;

        [SerializeField] private Animator _background;
        [SerializeField] private Animator _midground;
        [SerializeField] private Animator _bunker;

        private static int _dipperTrigget = Animator.StringToHash("Deeper");
        private static int _blinkTrigget = Animator.StringToHash("Blink");
        private static int _blinkLongTrigget = Animator.StringToHash("LongBlink");
        private static int _endTrigget = Animator.StringToHash("End");

        private float _timeToNextShake;
        private float _lastShakeTime;
        private bool  _isOver;

        private void OnEnable()
        {
            GameModel.Events.SubscribeOnGameStateChange(VisualizeNewState);
        }

        private void OnDisable()
        {
            GameModel.Events.UnsubscribeOnGameStateChange(VisualizeNewState);
        }

        private void Update()
        {
            if (Time.realtimeSinceStartup >= _timeToNextShake + _lastShakeTime && !_isOver)
            {
                _timeToNextShake = Random.Range(4, 13);
                _lastShakeTime = Time.realtimeSinceStartup;

                _shaker.ShakingTime = 0.6f;
                _shaker.Intensity = 0.5f;

                _shaker.StartShake();
                StartBlink();
            }
        }

        private void VisualizeNewState(Game_State_Change context)
        {
            _shaker.ShakingTime = 1f;
            _shaker.Intensity = 1f;
            _shaker.StartShake();

            _background.SetTrigger(_dipperTrigget);
            _midground.SetTrigger(_dipperTrigget);

            if (context.NewState == GameState.Over)
            {
                StartLastBlink();
                _isOver = true;
            }
            else
            {
                StartBlink();
            }
        }

        private void StartBlink()
        {
            _bunker.SetTrigger(Random.value < 0.5f ? _blinkTrigget : _blinkLongTrigget);
        }

        private void StartLastBlink()
        {
            _bunker.SetTrigger(_endTrigget);
        }
    }
}