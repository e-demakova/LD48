using UnityEngine;

namespace Deblue.LD48
{
    public class DippingVisualizator : UniqMono<DippingVisualizator>
    {
        [SerializeField] private CameraShaker _shaker;

        private void OnEnable()
        {
            GameModel.Events.SubscribeOnGameStateChange(VisualizeNewState);
        }

        private void OnDisable()
        {
            GameModel.Events.UnsubscribeOnGameStateChange(VisualizeNewState);
        }

        private void VisualizeNewState(Game_State_Change context)
        {
            _shaker.StartShake();
        }
    }
}