using Deblue.FSM;

namespace Deblue.LD48
{
    public class BoyInDialogState : GenericState<Boy>
    {
        public BoyInDialogState(Boy executor, StateMachine owner) : base(executor, owner)
        {
        }
    }
}