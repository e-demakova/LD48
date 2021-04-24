using Deblue.FSM;

namespace Deblue.LD48
{
    public class PlayerInDialogState : GenericState<Player>
    {
        public PlayerInDialogState(Player executor, StateMachine owner) : base(executor, owner)
        {
        }
    }
}