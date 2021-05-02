using System;

using UnityEngine;

using Deblue.ObservingSystem;
using Deblue.InputSystem;

namespace Deblue.GameProcess
{
    public class GameTime : UniqMono<GameTime>
    {
        public static float TimeSinceStartup => _time;
        private static float _time;

        public static bool IsPause => _isPause;
        private static bool _isPause;

        private static Handler<Game_Pause_Change> _pauseHandlers = new Handler<Game_Pause_Change>();

        private void OnDestroy()
        {
            _pauseHandlers.Clear();
        }

        private void Update()
        {
            if (!_isPause)
            {
                _time += Time.deltaTime;
            }
        }

        public static void SubscribeOnPause(Action<Game_Pause_Change> action)
        {
            _pauseHandlers.Subscribe(action);
        }

        public static void UnsubscribeOnPause(Action<Game_Pause_Change> action)
        {
            _pauseHandlers.Unsubscribe(action);
        }

        private void SetPause(On_Button_Down context)
        {
            if(_isPause == true)
            {
                return;
            }
            InputReciver.UnsubscribeOnInput<On_Button_Down>(SetPause, KeyCode.Escape);
            InputReciver.SubscribeOnInput<On_Button_Down>(Unpause, KeyCode.Escape);
            ChangePause();
        }

        private void Unpause(On_Button_Down context)
        {
            if (_isPause == false)
            {
                return;
            }
            InputReciver.UnsubscribeOnInput<On_Button_Down>(Unpause, KeyCode.Escape);
            InputReciver.SubscribeOnInput<On_Button_Down>(SetPause, KeyCode.Escape);
            ChangePause();
        }

        private static void ChangePause()
        {
            _isPause = !_isPause;
            _pauseHandlers.Raise(new Game_Pause_Change(_isPause));
        }
    }
}
