using System.Collections.Generic;

using UnityEngine;

using Deblue.ObservingSystem;

namespace Deblue.LD48
{
    public class Triggers : UniqMono<Triggers>
    {
        public static IReadOnlyHandler<Player_Outside> PlayerOutside => _playerOutside;
        public static IReadOnlyHandler<Player_On_Door> PlayerOnDoor => _playerOnDoor;

        private static Handler<Player_Outside> _playerOutside = new Handler<Player_Outside>();
        private static Handler<Player_On_Door> _playerOnDoor = new Handler<Player_On_Door>();

        [SerializeField] private Trigger2D _outsideTrigger;
        [SerializeField] private Trigger2D _insideTrigger;

        private List<IObserver> _observers = new List<IObserver>(2);

        protected override void MyAwake()
        {
            _outsideTrigger.SubscribeOnTrigger<Boy>(context => _playerOutside.Raise(new Player_Outside()), _observers);
            _insideTrigger.SubscribeOnTrigger<Boy>(context => _playerOnDoor.Raise(new Player_On_Door()), _observers);
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _observers.Count; i++)
            {
                _observers[i].Dispose();
            }
            _observers.Clear();
        }
    }

    public readonly struct Player_Outside
    {
    }

    public readonly struct Player_On_Door
    {
    }
}