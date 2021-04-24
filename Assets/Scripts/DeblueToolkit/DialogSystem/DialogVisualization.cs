using UnityEngine;
using TMPro;

using Deblue.ObservingSystem.Events;

namespace Deblue.DialogSystem
{
    [RequireComponent(typeof(Animator))]
    public class DialogVisualization : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _dialogText;

        private int IsOpen = Animator.StringToHash("IsOpen");
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            DialogSwitcher.Events.SubscribeOnReplicaSwitch(VisualizeNewReplica);
            DialogSwitcher.Events.SubscribeOnDialogStart(OpenWindow);
            DialogSwitcher.Events.SubscribeOnDialogEnd(CloseWindow);
        }

        private void OnDisable()
        {
            DialogSwitcher.Events.UnsubscribeOnReplicaSwitch(VisualizeNewReplica);
            DialogSwitcher.Events.UnsubscribeOnDialogStart(OpenWindow);
            DialogSwitcher.Events.UnsubscribeOnDialogEnd(CloseWindow);
        }

        private void CloseWindow(Dialog_End context)
        {
            _animator.SetBool(IsOpen, false);
        }

        private void OpenWindow(Dialog_Start context)
        {
            _animator.SetBool(IsOpen, true);
        }

        private void VisualizeNewReplica(Replica_Switch context)
        {
            _dialogText.text = context.Replica.Text;
        }
    }
}