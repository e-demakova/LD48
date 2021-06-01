using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine;

using TMPro;

using Deblue.Localization;
using Deblue.ObservingSystem;

namespace Deblue.DialogSystem
{
    [DefaultExecutionOrder(-900)]
    [RequireComponent(typeof(Animator))]
    public class DialogVisualization : UniqMono<DialogVisualization>
    {
        private static int _isOpen = Animator.StringToHash("IsOpen");

        [SerializeField] private TextMeshProUGUI  _replica;
        [SerializeField] private Image            _character;
        [SerializeField] private CharactersDataSO _charactersData;
        [SerializeField] private ChoiceView       _choicePrefab;
        [SerializeField] private Transform        _choicesConteiner;

        private DialogSwitcher   _switcher;
        private Animator         _animator;
        private List<ChoiceView> _choices = new List<ChoiceView>(4);
        private List<IObserver>  _observers = new List<IObserver>(4);

        protected override void MyAwake()
        {
            _animator = GetComponent<Animator>();
#if UNITY_EDITOR
            _charactersData.Serialize();
#endif
        }

        private void OnEnable()
        {
            DialogSwitcher.Events.SubscribeOnReplicaSwitch(VisualizeNewReplica, _observers);
            DialogSwitcher.Events.SubscribeOnDialogStart(OpenWindow, _observers);
            DialogSwitcher.Events.SubscribeOnDialogEnd(CloseWindow, _observers);
            DialogSwitcher.Events.SubscribeOnGiveChoice(VisualizeChoice, _observers);
        }

        private void OnDisable()
        {
            ObserverHalper.ClearObservers(_observers);
        }

        public void Init(DialogSwitcher switcher)
        {
            _switcher = switcher;
        }

        private void CloseWindow(Dialog_End context)
        {
            _animator.SetBool(_isOpen, false);
        }

        private void OpenWindow(Dialog_Start context)
        {
            _animator.SetBool(_isOpen, true);
        }

        private void VisualizeNewReplica(Replica_Switch context)
        {
            _replica.text = Localizator.GetText(context.Replica.TextId);
            var data = _charactersData.GetCharacter(context.Replica.CharacterId);
            _character.sprite = data.Icon;
        }

        private void VisualizeChoice(Dialog_Give_Choice context)
        {
            _replica.text = Localizator.GetText(context.ChoiceTextId);

            var choicesDelta = context.Choices.Length - _choices.Count;

            for (int i = 0; i < choicesDelta; i++)
            {
                var newChoice = Instantiate(_choicePrefab, _choicesConteiner);
                _choices.Add(newChoice);

                newChoice.ChoiceMaded.Subscribe(x =>
                {
                    _switcher.OnChoiceMade(x.Choice);
                    HideChoices();
                },
                _observers);
            }
            for (int i = 0; i < context.Choices.Length; i++)
            {
                _choices[i].SetChoice(context.Choices[i]);
                _choices[i].Show();
            }
        }

        private void HideChoices()
        {
            for (int i = 0; i < _choices.Count; i++)
            {
                _choices[i].Hide();
            }
        }
    }
}