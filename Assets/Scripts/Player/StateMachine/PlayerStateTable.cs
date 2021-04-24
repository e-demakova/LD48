using Deblue.FSM;

namespace Deblue.LD48
{
    public enum PlayerStateId
    {
        Move,
        OnStairs,
        InDialog
    }

    public sealed class PlayerStateTable : StatesTable<PlayerStateId>
    {
        private Player _player;

        public PlayerStateTable(StateMachine stateMachine, Player player) : base(stateMachine)
        {
            _player = player;
        }

        public void Init()
        {
            SwitchToNextFromAny(GetState(PlayerStateId.Move));
        }

        public override void TryChangeState()
        {
            var state = _stateMachine.CurrentState;

            //TODO: Сделать инициализацию всех стейтов при создании для избавления от этого треша "GetState".
            //TODO: Переделать "SwitchToNextFromAny" в просто "SwitchToNext".
            if (state != GetState(PlayerStateId.InDialog) && _player.IsTalk)
            {
                SwitchToNextFromAny(GetState(PlayerStateId.InDialog));
            }
            else if (state == GetState(PlayerStateId.InDialog) && !_player.IsTalk)
            {
                SwitchToNextFromAny(GetState(PlayerStateId.Move));
            }
            else if (state == GetState(PlayerStateId.OnStairs))
            {
                var stairsState = (PlayerOnStairsState)state;
                if (stairsState.IsNearOnBound && stairsState.IsTryExit)
                {
                    SwitchToNextFromAny(PlayerStateId.Move);
                }
            }
            else if (state == GetState(PlayerStateId.Move))
            {
                var stairsState = (PlayerMoveState)state;
                if (stairsState.IsTryEnterOnStairs && _player.Stairs != null)
                {
                    SwitchToNextFromAny(PlayerStateId.Move);
                }
            }
        }

        protected override BaseState CreateState(PlayerStateId id)
        {
            switch (id)
            {
                case PlayerStateId.Move:
                    return new PlayerMoveState(_player, _stateMachine);

                case PlayerStateId.OnStairs:
                    return new PlayerOnStairsState(_player, _stateMachine);

                case PlayerStateId.InDialog:
                    return new PlayerInDialogState(_player, _stateMachine);

                default:
                    throw new System.ArgumentException(string.Format("State for Id '{0}' not defined.", id));
            }
        }
    }
}