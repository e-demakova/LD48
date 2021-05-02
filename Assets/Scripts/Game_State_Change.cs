namespace Deblue.LD48
{
    public readonly struct Game_State_Change
    {
        public readonly GameState OldState;
        public readonly GameState NewState;

        public Game_State_Change(GameState oldState, GameState newState)
        {
            OldState = oldState;
            NewState = newState;
        }
    }
}