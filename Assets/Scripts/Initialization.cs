using UnityEngine;

using Deblue.DialogSystem;
using Deblue.Story;
using Deblue.Interactive;

namespace Deblue.LD48
{
    public class Initialization : UniqMono<Initialization>
    {
        [SerializeField] private DialogSwitcher         _dialogSwitcher;
        [SerializeField] private Storyteller            _storyteller;
        [SerializeField] private InteractObjectsInScene _intersctObjects;
        [SerializeField] private Boy                    _boy;

        protected override void MyAwake()
        {
            Localization.Localizator.Init();
            _intersctObjects.Init(_boy);

            var dialogRequester = new DialogRequester(_dialogSwitcher);
            _storyteller.Init(dialogRequester);
        }
    }
}