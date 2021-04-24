using UnityEngine;

using Deblue.FSM;
using Deblue.InputSystem;

namespace Deblue.LD48
{
    public class PlayerOnStairsState : GenericState<Player>
    {
        public bool IsTryExit => _isTryExit;
        public bool _isTryExit;

        public bool IsNearOnBound =>
            Mathf.Abs(_transform.position.y - _stairs.TopBound.y) <= 0.05f ||
            Mathf.Abs(_transform.position.y - _stairs.BotBound.y) <= 0.05f;

        private bool IsOnStairsTop =>
            Mathf.Abs(_transform.position.y - _stairs.TopBound.y) <
            Mathf.Abs(_transform.position.y - _stairs.BotBound.y);

        private Stairs    _stairs;
        private Transform _transform;
        private float     _speed;

        public PlayerOnStairsState(Player executor, StateMachine owner) : base(executor, owner)
        {
            _transform = executor.transform;
            _speed = executor.Speed;
        }

        protected override void AfterEnter()
        {
            base.AfterEnter();
            InputReciver.SubscribeOnInputAxis(Move);
            _stairs = _executor.Stairs;

            _transform.position = IsOnStairsTop ? _stairs.EnterTopPosition : _stairs.EnterBotPosition;
        }

        public override void DeInit()
        {
            base.DeInit();
            InputReciver.UnsubscribeOnInputAxis(Move);
            _transform.position = IsOnStairsTop ? _stairs.ExitTopPosition : _stairs.ExitBotPosition;
        }

        private void Move(Vector2 direction)
        {
            var deltaY =  direction.y * Time.fixedDeltaTime * _speed; ;
            var newPosition = _transform.position + new Vector3(0, deltaY, 0);

            if (newPosition.y <= _stairs.TopBound.y && newPosition.y >= _stairs.BotBound.y)
            {
                _transform.position = newPosition;
            }
            _isTryExit = direction.x > 0;
        }
    }
}

