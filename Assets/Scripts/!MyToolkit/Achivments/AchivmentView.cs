using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Deblue.UiViews;

namespace Deblue.Achivments
{
    public class AchivmentView :  ModelView<AchivmentModel>
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private TextMeshProUGUI _description;

        private static int UnlockTrigger = Animator.StringToHash("Unlock");

        protected override void MyInit()
        {
            //TODO: Подписаться на анлок атчивок.
        }

        public override void DeInit()
        {
        }

        private void ShowAchivment(Achivment_Unlock context)
        {
            _label.text = context.Label;
            _description.text = context.Description;
            _icon.sprite = context.Icon;
            _animator.SetTrigger(UnlockTrigger);
        }

        public override void Show()
        {
            //TODO: Добавить анимацию через DOTween.
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }

    public readonly struct Achivment_Unlock
    {
        public readonly string Label;
        public readonly string Description;
        public readonly Sprite Icon;

        public Achivment_Unlock(AchivmentsTable achivments, string id)
        {
            var achivment = achivments[id];
            Label = achivment.Label;
            Description = achivment.Description;
            Icon = achivment.Icon;
        }
    }
}
