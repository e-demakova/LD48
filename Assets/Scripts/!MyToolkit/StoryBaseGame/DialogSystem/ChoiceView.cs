using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Deblue.UiViews;
using Deblue.Localization;
using Deblue.ObservingSystem;

namespace Deblue.DialogSystem
{
    public class ChoiceView : View
    {
        public Choice Choice;

        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button          _button;

        [Header("Text colors")]
        [SerializeField] private Color _enabled;
        [SerializeField] private Color _disabled;

        private Handler<Dialog_Choice_Maded> _choiceMaded = new Handler<Dialog_Choice_Maded>();

        public IReadOnlyHandler<Dialog_Choice_Maded> ChoiceMaded => _choiceMaded;

        public void SetChoice(Choice choice)
        {
            Choice = choice;
            if (choice.IsAvalible)
            {
                _text.text = Localizator.GetText(choice.TextId);
            }
            else
            {
                _text.text = Localizator.GetText(choice.BlockedTextId);
            }
            EnableButton(choice.IsAvalible);
        }

        public void EnableButton(bool enabled)
        {
            _button.interactable = enabled;
            _text.color = enabled ? _enabled : _disabled;
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnChoiceMade()
        {
            _choiceMaded.Raise(new Dialog_Choice_Maded(Choice));
        }
    }
}