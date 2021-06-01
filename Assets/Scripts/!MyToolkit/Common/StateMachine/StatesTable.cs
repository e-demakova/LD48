using System;
using System.Collections.Generic;

namespace Deblue.FSM
{
    public interface IStatesTable
    {
        void TryChangeState();
    }

    public abstract class StatesTable<TEnum> : IStatesTable where TEnum : Enum
    {
        protected StateMachine _stateMachine;
        protected Dictionary<TEnum, BaseState> _states = new Dictionary<TEnum, BaseState>(5);

        public StatesTable(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public abstract void TryChangeState();

        protected BaseState GetState(TEnum id)
        {
            if (!_states.TryGetValue(id, out var state))
            {
                state = CreateState(id);
                _states.Add(id, state);
            }
            return state;
        }

        protected abstract BaseState CreateState(TEnum id);

        protected void SetNextFromAny(TEnum id)
        {
            SetNextFromAny(GetState(id));
        }

        protected void SetNextFromAny(BaseState state)
        {
            _stateMachine.SetNextState(state);
        }

        protected void SwitchToNextFromAny(TEnum id)
        {
            SwitchToNextFromAny(GetState(id));
        }

        protected void SwitchToNextFromAny(BaseState state)
        {
            _stateMachine.SetNextState(state);
            _stateMachine.SwitchStateToNext();
        }

        protected void SetNextFromConcrete(TEnum fromId, TEnum toId)
        {
            SetNextFromConcrete(GetState(fromId), GetState(toId));
        }

        protected void SetNextFromConcrete(BaseState fromState, BaseState toState)
        {
            if (_stateMachine.CurrentState.Equals(fromState))
            {
                _stateMachine.SetNextState(toState);
            }
        }
       
        protected void SwitchToNextFromConcrete(TEnum fromId, TEnum toId)
        {
            SwitchToNextFromConcrete(GetState(fromId), GetState(toId));
        }

        protected void SwitchToNextFromConcrete(BaseState fromState, BaseState toState)
        {
            if (_stateMachine.CurrentState.Equals(fromState))
            {
                _stateMachine.SetNextState(toState);
                _stateMachine.SwitchStateToNext();
            }
        }
    }
}