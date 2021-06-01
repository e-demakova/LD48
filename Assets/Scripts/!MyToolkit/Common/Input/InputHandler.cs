using System.Collections.Generic;
using System;

using UnityEngine;

using Deblue.ObservingSystem;

namespace Deblue.InputSystem
{
    //Можно было бы наследоваться от EventSender, но тут все ивенты заранее предопределены
    //и вызываться будут скорее всего довольно часто, так что для избежания лишних проверок
    //было решено не связывать эти классы.
    public class InputHandler
    {
        public KeyCode KeyCode;

        private readonly Handler<On_Button_Down> _onButtonDown = new  Handler<On_Button_Down>();
        private readonly Handler<On_Button>      _onButton     = new  Handler<On_Button>();
        private readonly Handler<On_Button_Up>   _onButtonUp   = new  Handler<On_Button_Up>();

        public InputHandler(KeyCode keyCode)
        {
            KeyCode = keyCode;
        }

        public void OnButtonDown()
        {
            if (Input.GetKeyDown(KeyCode))
            {
                _onButtonDown.Raise(new On_Button_Down());
            }
        }

        public void OnButton()
        {
            if (Input.GetKey(KeyCode))
            {
                _onButton.Raise(new On_Button());
            }
        }

        public void OnButtonUp()
        {
            if (Input.GetKeyUp(KeyCode))
            {
                _onButtonUp.Raise(new On_Button_Up());
            }
        }

        #region Subscribing
        public void Subscribe<T>(Action<T> action, List<IObserver> observers = null) where T : struct
        {
            var actionOnDown = action as Action<On_Button_Down>;
            if (actionOnDown != null)
            {
                SubscribeOnButtonDown(actionOnDown, observers);
                return;
            }

            var actionOnButton = action as Action<On_Button>;
            if (actionOnButton != null)
            {
                SubscribeOnButton(actionOnButton, observers);
                return;
            }

            var actionOnUp = action as Action<On_Button_Up>;
            if (actionOnUp != null)
            {
                SubscribeOnButtonUp(actionOnUp, observers);
                return;
            }

            ThrowUnexpectedTypeException(typeof(T));
        }

        public void SubscribeOnButtonDown(Action<On_Button_Down> action, List<IObserver> observers = null)
        {
            _onButtonDown.Subscribe(action, observers);
        }

        public void SubscribeOnButton(Action<On_Button> action, List<IObserver> observers = null)
        {
            _onButton.Subscribe(action, observers);
        }

        public void SubscribeOnButtonUp(Action<On_Button_Up> action, List<IObserver> observers = null)
        {
            _onButtonUp.Subscribe(action, observers);
        }
        #endregion

        #region Unsubscribing
        public void UnsubscribeAll()
        {
            _onButtonDown.Clear();
            _onButton.Clear();
            _onButtonUp.Clear();
        }

        public void Unsubscribe<T>(Action<T> action) where T : struct
        {
            var actionOnDown = action as Action<On_Button_Down>;
            if (actionOnDown != null)
            {
                UnsubscribeOnButtonDown(actionOnDown);
                return;
            }

            var actionOnButton = action as Action<On_Button>;
            if (actionOnButton != null)
            {
                UnsubscribeOnButton(actionOnButton);
                return;
            }

            var actionOnUp = action as Action<On_Button_Up>;
            if (actionOnUp != null)
            {
                UnsubscribeOnButtonUp(actionOnUp);
                return;
            }

            ThrowUnexpectedTypeException(typeof(T));
        }

        public void UnsubscribeOnButtonDown(Action<On_Button_Down> action)
        {
            _onButtonDown.Unsubscribe(action);
        }

        public void UnsubscribeOnButton(Action<On_Button> action)
        {
            _onButton.Unsubscribe(action);
        }

        public void UnsubscribeOnButtonUp(Action<On_Button_Up> action)
        {
            _onButtonUp.Unsubscribe(action);
        }
        #endregion

        private void ThrowUnexpectedTypeException(Type type)
        {
            throw new ArgumentException($"Unexpected input type {type}.");
        }
    }
}
