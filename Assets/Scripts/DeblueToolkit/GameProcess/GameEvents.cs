namespace Deblue.GameProcess
{
    public readonly struct Game_Start
    {
    }

    public readonly struct Game_Pause_Change
    {
        public readonly bool IsPaused;

        public Game_Pause_Change(bool isPaused)
        {
            IsPaused = isPaused;
        }
    }

    public readonly struct Game_Over
    {
    }

    public readonly struct New_Game
    {
    }

    public readonly struct New_Game_Session
    {
    }

    public readonly struct End_Game_Session
    {
    }

    public readonly struct Level_Up
    {
        public readonly int NewLevel;

        public Level_Up(int level)
        {
            NewLevel = level;
        }
    }
}
