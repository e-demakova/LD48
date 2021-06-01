using UnityEngine;

using Deblue.GameProcess;
using Deblue.ObservingSystem;
using Deblue.DialogSystem;
using Deblue.LD48;

namespace Deblue.Story
{
    public class Storyteller : PersistentMono<Storyteller>
    {
        [SerializeField] private StepSO _currentStep;

        private IObserver       _pauseObserver;
        private bool            _isPaused;
        private DialogRequester _dialogRequester;
        private GameModel       _gameModel;

        public void Init(DialogRequester dialogRequester, GameModel gameModel)
        {
            _dialogRequester = dialogRequester;
            _dialogRequester.SetAvalibleDialogues(_currentStep.AvalibleCharacters);
            _currentStep.Init();
            _gameModel = gameModel;
        }

        protected override void MyAwake()
        {
            _pauseObserver = GameTime.SubscribeOnPause(context => _isPaused = context.IsPaused);
        }

        private void OnDestroy()
        {
            _pauseObserver.Dispose();
            _currentStep.DeInit();
        }

        private void FixedUpdate()
        {
            if (!_isPaused)
            {
                _currentStep.Execute(Time.fixedDeltaTime);
                if (_currentStep.IsDone)
                {
                    OnStepChange();
                }
            }
        }

        private void OnStepChange()
        {
            _currentStep.DeInit();
            _currentStep = _currentStep.NextStep;
            _dialogRequester.SetAvalibleDialogues(_currentStep.AvalibleCharacters);
            _currentStep.Init();
            if(_currentStep.GameState != GameState.Default)
            {
                _gameModel.ChangeState(_currentStep.GameState);
            }
        }
    }
}