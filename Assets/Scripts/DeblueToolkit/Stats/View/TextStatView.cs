using UnityEngine;

using TMPro;

using Deblue.ObservingSystem;

namespace Deblue.Stats
{
    public class TextStatView<TEnum> : StatView<TEnum> where TEnum : System.Enum
    {
        [SerializeField] private TextMeshProUGUI _textMesh;

        protected override void Init()
        {
            _textMesh.text = _property.Value.ToString();
        }

        public override void UpdateView(Property_Changed<float> context)
        {
            _textMesh.text = context.NewValue.ToString();
        }
    }
}