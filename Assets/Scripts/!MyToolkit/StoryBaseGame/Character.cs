using UnityEngine;

using Deblue.ObservingSystem;

namespace Deblue.DialogSystem
{
    public readonly struct Dialog_Trigger
    {
        public readonly Character Character;
        public readonly bool      IsEnter;

        public Dialog_Trigger(Character character, bool isEnter)
        {
            Character = character;
            IsEnter = isEnter;
        }
    }

    public readonly struct Player_Give_Item
    {
        public readonly Character Character;
        public readonly string    ItemId;

        public Player_Give_Item(Character character, string itemId)
        {
            Character = character;
            ItemId = itemId;
        }
    }

    public interface IDialogStarter
    {
    }

    [RequireComponent(typeof(Collider2D))]
    public class Character : MonoBehaviour
    {
        [SerializeField] private string _characterId;

        private Handler<Dialog_Trigger> _playerTalk = new Handler<Dialog_Trigger>();

        public IReadOnlyHandler<Dialog_Trigger> PlayerTalk => _playerTalk;
        public string CharacterId => _characterId;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IDialogStarter>(out var dialogStarter))
            {
                _playerTalk.Raise(new Dialog_Trigger(this, true));
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<IDialogStarter>(out var dialogStarter))
            {
                _playerTalk.Raise(new Dialog_Trigger(this, false));
            }
        }
    }
}