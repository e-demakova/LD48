using UnityEngine;

using Deblue.DialogSystem;
using Deblue.Story;

namespace Deblue.LD48
{
    public class Initialization : UniqMono<Initialization>
    {
        [SerializeField] private DialogSwitcher _dialogSwitcher;
        [SerializeField] private Storyteller    _storyteller;

        protected override void MyAwake()
        {
            var dialogRequester = new DialogRequester(_dialogSwitcher);
            _storyteller.Init(dialogRequester);
        }
    }
}