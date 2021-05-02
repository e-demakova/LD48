using UnityEngine;

namespace Deblue.LD48
{
    [RequireComponent(typeof(Collider2D))]
    public class Stairs : MonoBehaviour
    {
        public bool IsBlocked { get; private set; }

        public Vector3 BotBound => YToPosition(_botBound);
        public Vector3 TopBound => YToPosition(_topBound);

        public Vector3 ExitBotPosition => YToPosition(_exitBotPosition);
        public Vector3 ExitTopPosition => YToPosition(_exitTopPosition);

        [SerializeField] private float _gizmosWeight;
        [Space]
        [SerializeField] private float _botBound;
        [SerializeField] private float _topBound;
        [Space]
        [SerializeField] private float _exitBotPosition;
        [SerializeField] private float _exitTopPosition;

        private void Awake()
        {
            LD48GameModel.Events.SubscribeOnGameStateChange(CheckIsBlocked);
        }

        private void OnDestroy()
        {
            LD48GameModel.Events.UnsubscribeOnGameStateChange(CheckIsBlocked);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawCube(YToPosition(_topBound), Vector3.one * _gizmosWeight);
            Gizmos.DrawCube(YToPosition(_botBound), Vector3.one * _gizmosWeight);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(YToPosition(_exitTopPosition), Vector3.one * _gizmosWeight);
            Gizmos.DrawCube(YToPosition(_exitBotPosition), Vector3.one * _gizmosWeight);
        }

        private void CheckIsBlocked(Game_State_Change context)
        {
            if(context.NewState == GameState.Deeper)
            {
                IsBlocked = true;
            }
        }

        private Vector3 YToPosition(float y)
        {
            return new Vector3(transform.position.x, y, transform.position.z);
        }
    }
}