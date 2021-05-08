using Deblue.FSM;

namespace Deblue.LD48
{
    public enum BoyStateId
    {
        Move,
        OnStairs,
        InDialog
    }

    public sealed class BoyStateTable : StatesTable<BoyStateId>
    {
        private Boy _player;

        public BoyStateTable(StateMachine stateMachine, Boy player) : base(stateMachine)
        {
            _player = player;
        }

        public void Init()
        {
            SwitchToNextFromAny(GetState(BoyStateId.Move));
        }

        public override void TryChangeState()
        {
            var state = _stateMachine.CurrentState;

            //TODO: Сделать инициализацию всех стейтов при создании для избавления от этого треша "GetState".
            //TODO: Переделать "SwitchToNextFromAny" в просто "SwitchToNext".
            if (state != GetState(BoyStateId.InDialog) && _player.IsTalk)
            {
                SwitchToNextFromAny(GetState(BoyStateId.InDialog));
            }
            else if (state == GetState(BoyStateId.InDialog) && !_player.IsTalk)
            {
                SwitchToNextFromAny(GetState(BoyStateId.Move));
            }
            else if (state == GetState(BoyStateId.OnStairs))
            {
                var stairsState = (BoyOnStairsState)state;
                if (!stairsState.IsOnStairs || (stairsState.IsNearOnBound && stairsState.IsTryExit))
                {
                    SwitchToNextFromAny(BoyStateId.Move);
                }
            }
            else if (state == GetState(BoyStateId.Move))
            {
                var moveState = (BoyMoveState)state;
                if (moveState.IsTryEnterOnStairs)
                {
                    SwitchToNextFromAny(BoyStateId.OnStairs);
                }
            }
        }

        protected override BaseState CreateState(BoyStateId id)
        {
            switch (id)
            {
                case BoyStateId.Move:
                    return new BoyMoveState(_player, _stateMachine);

                case BoyStateId.OnStairs:
                    return new BoyOnStairsState(_player, _stateMachine);

                case BoyStateId.InDialog:
                    return new BoyInDialogState(_player, _stateMachine);

                default:
                    throw new System.ArgumentException(string.Format("State for Id '{0}' not defined.", id));
            }
        }
    }
}