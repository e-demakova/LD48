using UnityEngine;
using Deblue.GameProcess;

namespace Deblue.LD48
{
    public class GameModel : PersistentMono<GameModel>
    {
        public static GlobalGameEventsLD48 _events = new GlobalGameEventsLD48();
        public static IGlobalGameEventsLD48 Events => _events;

        private static GameState _state;
        public static GameState State => _state;

        private Coroutine _testGameStateChanger;

        private void OnDestroy()
        {
            _events.ClearSubscribers();
        }

        public void ChangeStateToNext()
        {
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
                    break;

                default:
                    break;
            }
        }

        public void ChangeState(GameState newState)
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