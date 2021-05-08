using UnityEngine;

using Deblue.FSM;
using Deblue.InputSystem;

namespace Deblue.LD48
{
    public sealed class BoyMoveState : GenericState<Boy>
    {
        public bool IsTryEnterOnStairs => _isTryEnterOnStairs;

        private bool _isTryEnterOnStairs;

        private static int _isMove = Animator.StringToHash("IsMove");

        private Animator  _animator;
        private Transform _transform;
        private float     _speed;

        public BoyMoveState(Boy executor, StateMachine owner) : base(executor, owner)
        {
            _animator = executor.Animator;
            _transform = executor.transform;
            _speed = executor.Speed;
        }

        protected override void AfterEnter()
        {
            base.AfterEnter();
            InputReciver.SubscribeOnInputAxis(Move);
        }

        public override void DeInit()
        {
            base.DeInit();
            _animator.SetBool(_isMove, false);
            InputReciver.UnsubscribeOnInputAxis(Move);
        }

        private void Move(Vector2 direction)
        {
            var deltaX = direction.x * Time.fixedDeltaTime * _speed;

            if (deltaX != 0)
            {
                _transform.position += new Vector3(deltaX, 0, 0);
                _animator.SetBool(_isMove, true);
                _transform.rotation = deltaX > 0 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
            }
            else
            {
                _animator.SetBool(_isMove, false);
            }

            var stairs = _executor.Stairs;
            if (stairs != null)
            {
                if (!stairs.IsBlocked)
                {
                    var isOnTopBound = Mathf.Abs(stairs.BotBound.y - _transform.position.y) >
                                       Mathf.Abs(stairs.TopBound.y - _transform.position.y);

                    _isTryEnterOnStairs = isOnTopBound ? direction.y < 0 : direction.y > 0;
                    return;
                }
            }
            _isTryEnterOnStairs = false;
        }
    }
}