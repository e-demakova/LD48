using System;
using System.Collections.Generic;

using Deblue.ObservingSystem;

namespace Deblue.GameProcess
{
    public interface IGlobalGameEvents
    {
        IObserver SubscribeOnGameOver(Action<Game_Over> action, List<IObserver> observers = null);
        IObserver SubscribeOnNewGame(Action<New_Game> action, List<IObserver> observers = null);
        (IObserver, IObserver) SubscribeOnNewSession(Action<New_Game_Session> newAction, Action<End_Game_Session> endAction);
        (IObserver, IObserver) SubscribeOnNewSession(Action<New_Game_Session> newAction, List<IObserver> newObservers, Action<End_Game_Session> endAction, List<IObserver> endObservers);

        void UnsubscribeOnGameOver(Action<Game_Over> action);
        void UnsubscribeOnNewGame(Action<New_Game> action);
        void UnsubscribeOnNewSession(Action<New_Game_Session> newAction, Action<End_Game_Session> endAction);
    }

    public class GlobalGameEvents : EventSender, IGlobalGameEvents
    {
        #region Subscribing
        public IObserver SubscribeOnGameOver(Action<Game_Over> action, List<IObserver> observers = null)
        {
            return Subscribe(action, observers);
        }

        public IObserver SubscribeOnNewGame(Action<New_Game> action, List<IObserver> observers = null)
        {
            return Subscribe(action, observers);
        }

        public (IObserver, IObserver) SubscribeOnNewSession(Action<New_Game_Session> newAction, Action<End_Game_Session> endAction)
        {
            var a = Subscribe(newAction);
            var b = Subscribe(endAction);
            return (a, b);
        }
        
        public (IObserver, IObserver) SubscribeOnNewSession(Action<New_Game_Session> newAction, List<IObserver> newObservers, Action<End_Game_Session> endAction, List<IObserver> endObservers)
        {
            var a = Subscribe(newAction, newObservers);
            var b = Subscribe(endAction, endObservers);
            return (a, b);
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