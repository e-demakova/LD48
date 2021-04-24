using UnityEngine;

using Deblue.FSM;
using Deblue.InputSystem;

namespace Deblue.LD48
{
    public sealed class PlayerMoveState : GenericState<Player> 
    {
        public bool IsTryEnterOnStairs => _isTryEnterOnStairs;
        private bool _isTryEnterOnStairs;

        private Transform _transform;
        private float     _speed;

        public PlayerMoveState(Player executor, StateMachine owner) : base(executor, owner)
        {
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
            InputReciver.UnsubscribeOnInputAxis(Move);
        }

        private void Move(Vector2 direction)
        {
            var deltaX =  direction.x * Time.fixedDeltaTime * _speed; ;
            _transform.position += new Vector3(deltaX, 0, 0);
        }
    }
}