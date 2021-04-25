using System.Collections;

using UnityEngine;

namespace Deblue.LD48
{
    public class GameModel : PersistentMono<GameModel>
    {
        public static GlobalGameEvents _events = new GlobalGameEvents();
        public static IGlobalGameEvents Events => _events;

        private static GameState _state;
        public static GameState State => _state;

        protected override void MyAwake()
        {
            StartCoroutine(TestGameState());
        }

        private void OnDestroy()
        {
            _events.ClearSubscribers();
        }

        private IEnumerator TestGameState()
        {
            var lastStateTime = Time.realtimeSinceStartup;

            while (true)
            {
                if (Time.realtimeSinceStartup > lastStateTime + 3f)
                {
                    lastStateTime = Time.realtimeSinceStartup;
                    switch (_state)
                    {
                        case GameState.Play:
                            ChangeState(GameState.Deep);
                            break;

                        case GameState.Deep:
                            ChangeState(GameState.Deeper);
                            break;

                        case GameState.Deeper:
                            ChangeState(GameState.AndDeeper);
                            break;

                        case GameState.AndDeeper:
                            ChangeState(GameState.Over);
                            StopAllCoroutines();
                            break;

                        case GameState.Over:
                            break;

                        default:
                            break;
                    }
                }
                yield return new WaitForFixedUpdate();
            }
        }

        private void ChangeState(GameState newState)
        {
            var oldState = _state;
            _state = newState;
            _events.Raise(new Game_State_Change(oldState, _state));
        }

        private void OnGameOver()
        {
            _state = GameState.Over;
            _events.Raise(new Game_Over());
        }

        private void OnNewGame()
        {
            _events.Raise(new New_Game());
        }

        private void OnNewSession()
        {
            _events.Raise(new New_Game_Session());
        }
        
        private void OnEndSession()
        {
            _events.Raise(new End_Game_Session());
        }
    }
}