using UnityEngine;

using Deblue.FSM;
using Deblue.InputSystem;

namespace Deblue.LD48
{
    public class BoyOnStairsState : GenericState<Boy>
    {
        private Stairs    _stairs;
        private Transform _transform;
        private float     _speed;
        private bool      _isTryExit;

        public bool IsOnStairs => _stairs != null;
        public bool IsTryExit => _isTryExit;

        public bool IsNearOnBound => IsOnStairs ? Mathf.Abs(_transform.position.y - _stairs.TopBound.y) <= 0.05f ||
                                                  Mathf.Abs(_transform.position.y - _stairs.BotBound.y) <= 0.05f
                                                  : false;

        private bool IsOnStairsTop => IsOnStairs ? Mathf.Abs(_transform.position.y - _stairs.TopBound.y) <
                                                   Mathf.Abs(_transform.position.y - _stairs.BotBound.y)
                                                   : false;

        public BoyOnStairsState(Boy executor, StateMachine owner) : base(executor, owner)
        {
            _transform = executor.transform;
            _speed = executor.Speed;
        }

        protected override void AfterEnter()
        {
            base.AfterEnter();
            InputReciver.SubscribeOnInputAxis(Move);
            _stairs = _executor.Stairs;
            if (IsOnStairs)
            {
                _transform.position = IsOnStairsTop ? _stairs.TopBound : _stairs.BotBound;
            }
        }

        public override void DeInit()
        {
            base.DeInit();
            InputReciver.UnsubscribeOnInputAxis(Move);
            if (IsOnStairs)
            {
                _transform.position = IsOnStairsTop ? _stairs.ExitTopPosition : _stairs.ExitBotPosition;
            }
        }

        private void Move(Vector2 direction)
        {
            if (!IsOnStairs)
            {
                return;
            }
            var deltaY =  direction.y * Time.fixedDeltaTime * _speed; ;
            var newPosition = _transform.position + new Vector3(0, deltaY, 0);

            var isMoveInsideStairs = newPosition.y <= _stairs.TopBound.y && newPosition.y >= _stairs.BotBound.y;

            if (isMoveInsideStairs)
            {
                _transform.position = newPosition;
            }
            _isTryExit = direction.x != 0 && (!isMoveInsideStairs || (IsNearOnBound && direction.y == 0));
        }
    }
}

