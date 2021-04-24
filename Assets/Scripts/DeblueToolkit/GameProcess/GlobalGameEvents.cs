using System;

using Deblue.ObservingSystem;

namespace Deblue.GameProcess
{
    public interface IGlobalGameEvents
    {
        void SubscribeOnGameOver(Action<Game_Over> action);
        void SubscribeOnNewGame(Action<New_Game> action);
        void SubscribeOnNewSession(Action<New_Game_Session> newAction, Action<End_Game_Session> endAction);

        void UnsubscribeOnGameOver(Action<Game_Over> action);
        void UnsubscribeOnNewGame(Action<New_Game> action);
        void UnsubscribeOnNewSession(Action<New_Game_Session> newAction, Action<End_Game_Session> endAction);
    }

    public class GlobalGameEvents : EventSender, IGlobalGameEvents
    {
        #region Subscribing
        public void SubscribeOnGameOver(Action<Game_Over> action)
        {
            Subscribe(action);
        }
        
        public void SubscribeOnNewGame(Action<New_Game> action)
        {
            Subscribe(action);
        }
        
        public void SubscribeOnNewSession(Action<New_Game_Session> newAction, Action<End_Game_Session> endAction)
        {
            Subscribe(newAction);
            Subscribe(endAction);
        }
        #endregion

        #region Unsubscribing
        public void UnsubscribeOnGameOver(Action<Game_Over> action)
        {
            Unsubscribe(action);
        }
        
        public void UnsubscribeOnNewGame(Action<New_Game> action)
        {
            Unsubscribe(action);
        }

        public void UnsubscribeOnNewSession(Action<New_Game_Session> newAction, Action<End_Game_Session> endAction)
        {
            Unsubscribe(newAction);
            Unsubscribe(endAction);
        }
        #endregion
    }
}