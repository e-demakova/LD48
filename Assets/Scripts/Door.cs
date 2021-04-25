using UnityEngine;

namespace Deblue.LD48
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class Door : MonoBehaviour
    {
        [SerializeField] private Sprite     _close;
        [SerializeField] private Sprite     _open;
        [SerializeField] private Collider2D _closeCollider;

        private SpriteRenderer _renderer;
        private bool           _isBlocked;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            Close();

            GameModel.Events.SubscribeOnGameStateChange(BlockDoor);
        }

        private void OnDestroy()
        {
            GameModel.Events.UnsubscribeOnGameStateChange(BlockDoor);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out var palyer) && !_isBlocked)
            {
                Open();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out var palyer))
            {
                Close();
            }
        }

        private void Open()
        {
            _renderer.sprite = _open;
            _closeCollider.enabled = false;
        }

        private void Close()
        {
            _renderer.sprite = _close;
            _closeCollider.enabled = true;
        }

        private void BlockDoor(Game_State_Change context)
        {
            if (context.NewState == GameState.AndDeeper)
            {
                _isBlocked = true;
            }
        }
    }
}