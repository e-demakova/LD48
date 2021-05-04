using System;

using Deblue.GameProcess;

namespace Deblue.LD48
{
    public interface IGlobalGameEventsLD48 : IGlobalGameEvents
    {
        void SubscribeOnGameStateChange(Action<Game_State_Change> action);
        void UnsubscribeOnGameStateChange(Action<Game_State_Change> action);
    }

    public class GlobalGameEventsLD48 : GlobalGameEvents, IGlobalGameEventsLD48
    {
        public void SubscribeOnGameStateChange(Action<Game_State_Change> action)
        {
            Subscribe(action);
        }

        public void UnsubscribeOnGameStateChange(Action<Game_State_Change> action)
        {
            Unsubscribe(action);
        }
    }
}