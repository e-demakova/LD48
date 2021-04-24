using UnityEngine;

using Deblue.GameProcess;

namespace Deblue.FSM
{
    public enum StateStatus
    {
        Defoult,
        Enter,
        Active,
        Exit
    }

    public interface IState
    {
        void Init();

        void Execute();

        void DeInit();
    }

    public abstract class BaseState : IState
    {
        public bool IsLooping => _activeTime < 0;
        public bool StateTimeEnd;

        protected StateStatus     _status = StateStatus.Defoult;
        protected StateMachine    _owner;

        protected float _activeTime = -1;
        protected float _enterTime = -1;
        protected float _exitTime  = -1;
        protected float _markTime;

        //Вызывается стейтмашиной
        public abstract void DeInit();//При вызове переходит в "Exit" стейт
        public abstract void Init();//При вызове переходит в "Enter" стейет
        public abstract void Execute();

        //Вызывается самим скриптом
        protected abstract void AfterEnter();//Осуществляет переход из статуса "Enter" в "Active".
        protected abstract void AfterExit(); //Осуществляет переход из статуса "Exit" в "Defoult".
        protected abstract void PlayActiveBehaviour();
        protected abstract void PlayExitTransition();
        protected abstract void PlayEnterTransition();
    }

    public abstract class GenericState<T> : BaseState where T : MonoBehaviour
    {
        protected T _executor;

        public GenericState(T executor, StateMachine owner)
        {
            _executor = executor;
            _owner = owner;
        }

        public override void Init()
        {
            _markTime = GameTime.TimeSinceStartup;
            _status = StateStatus.Enter;
        }

        public override void DeInit()
        {
            _markTime = GameTime.TimeSinceStartup;
            _status = StateStatus.Exit;
            StateTimeEnd = false;
        }

        protected override void AfterEnter()
        {
            _markTime = GameTime.TimeSinceStartup;
            _status = StateStatus.Active;
        }

        public override void Execute()
        {
            switch (_status)
            {
                case StateStatus.Enter:
                    PlayEnterTransition();
                    if (GameTime.TimeSinceStartup >= _markTime + _enterTime)
                    {
                        AfterEnter();
                    }
                    break;

                case StateStatus.Active:
                    if (!IsLooping && GameTime.TimeSinceStartup >= _markTime + _activeTime)
                    {
                        StateTimeEnd = true;
                    }
                    else
                    {
                        PlayActiveBehaviour();
                    }
                    break;

                case StateStatus.Exit:
                    PlayExitTransition();
                    if (GameTime.TimeSinceStartup >= _markTime + _exitTime)
                    {
                        AfterExit();
                    }
                    break;
            }
        }

        protected override void AfterExit()
        {
            _owner.SwitchStateToNext();
            _status = StateStatus.Defoult;
        }

        protected override void PlayEnterTransition()
        {
        }

        protected override void PlayExitTransition()
        {
        }

        protected override void PlayActiveBehaviour()
        {
        }
    }
}