using UnityEngine;

using Deblue.DialogSystem;
using Deblue.Story;
using Deblue.Interactive;
using Deblue.Localization;

namespace Deblue.LD48
{
    public class Initialization : UniqMono<Initialization>
    {
        [SerializeField] private GameModel              _gameModel;
        [SerializeField] private DialogSwitcher         _dialogSwitcher;
        [SerializeField] private DialogVisualization    _dialogVisualization;
        [SerializeField] private Storyteller            _storyteller;
        [SerializeField] private InteractObjectsInScene _intersctObjects;
        [SerializeField] private Boy                    _boy;

        protected override void MyAwake()
        {
            Localizator.Init();
            _intersctObjects.Init(_boy);

            _dialogSwitcher.Init(new ChoiceChecker());
            var dialogRequester = new DialogRequester(_dialogSwitcher);
            _dialogVisualization.Init(_dialogSwitcher);
            _storyteller.Init(dialogRequester, _gameModel);
        }
    }
}