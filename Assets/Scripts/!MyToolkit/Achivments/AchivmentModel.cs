using System;

using Deblue.Models;
using Deblue.ObservingSystem;

namespace Deblue.Achivments
{
    public class AchivmentModel : IModel
    {
        private static Handler<Achivment_Unlock> _handler;

        public static void SubscribeOnAchivmentUnlock(Action<Achivment_Unlock> action)
        {
            _handler.Subscribe(action);
        }

        public static void UnsubscribeOnAchivmentUnlock(Action<Achivment_Unlock> action)
        {
            _handler.Subscribe(action);
        }

        public void DeInit()
        {
            _handler.Clear();
        }
    }
}
