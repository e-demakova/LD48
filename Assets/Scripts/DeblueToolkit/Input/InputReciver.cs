using System;
using System.Collections.Generic;

using UnityEngine;

using Deblue.ObservingSystem;

namespace Deblue.InputSystem
{
    public sealed class InputReciver : MonoBehaviour
    {
        public const string X_AXIS = "Horizontal";
        public const string Y_AXIS = "Vertical";

        private readonly static Dictionary<KeyCode, InputHandler> _handlers      = new Dictionary<KeyCode, InputHandler>(5);
        private readonly static List<InputHandler>                _inputHandlers = new List<InputHandler>(5);

        private readonly static Handler<Vector2>                  _axisHandlers  = new Handler<Vector2>();
        private readonly static Handler<Vector2>                  _mouseHandlers = new Handler<Vector2>();

        private void Update()
        {
            for (int i = 0; i < _inputHandlers.Count; i++)
            {
                _inputHandlers[i].OnButtonDown();
                _inputHandlers[i].OnButton();
                _inputHandlers[i].OnButtonUp();
            }
        }

        private void FixedUpdate()
        {
            var x = Input.GetAxis(X_AXIS);
            var y = Input.GetAxis(Y_AXIS);
            _axisHandlers.Raise(new Vector2(x, y));

            var mousePosition = Input.mousePosition;
            mousePosition = MainCamera.Camera.ScreenToWorldPoint(mousePosition);
            _mouseHandlers.Raise(mousePosition);
        }

        private void OnDisable()
        {
            for (int i = 0; i < _inputHandlers.Count; i++)
            {
                _inputHandlers[i].UnsubscribeAll();
            }
            _axisHandlers.Clear();
        }

        public static void SubscribeOnInputAxis(Action<Vector2> action)
        {
            _axisHandlers.Subscribe(action);
        }

        public static void SubscribeOnMousePosition(Action<Vector2> action)
        {
            _mouseHandlers.Subscribe(action);
        }

        public static void SubscribeOnInput<T>(Action<T> action, KeyCode keyCode) where T : struct
        {
            if (!_handlers.TryGetValue(keyCode, out var handler))
            {
                handler = new InputHandler(keyCode);
                _handlers.Add(keyCode, handler);
                _inputHandlers.Add(handler);
            }
            handler.Subscribe(action);
        }

        public static void UnsubscribeOnInputAxis(Action<Vector2> action)
        {
            _axisHandlers.Unsubscribe(action);
        }

        public static void UnsubscribeOnMousePosition(Action<Vector2> action)
        {
            _mouseHandlers.Unsubscribe(action);
        }

        public static void UnsubscribeOnInput<T>(Action<T> action, KeyCode keyCode) where T : struct
        {
            if (_handlers.TryGetValue(keyCode, out var handler))
            {
                handler.Unsubscribe(action);
            }
        }
    }
}
