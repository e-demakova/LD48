using Deblue.DialogSystem;

namespace Deblue.GameProcess
{
    public class GameModel : PersistentMono<GameModel>
    {
        public static GlobalGameEvents _events = new GlobalGameEvents();
        public static IGlobalGameEvents Events => _events;

        private static GameState _state;
        public static GameState State => _state;

        private void OnDestroy()
        {
            _events.ClearSubscribers();
        }

        private void OnGameOver(Dialogues_Over context)
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