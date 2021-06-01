using UnityEngine;

namespace Deblue.DialogSystem
{
    [System.Serializable]
    public class Replica
    {
        [SerializeField] private string    _textId;
        [SerializeField] private string    _characterId;
        [SerializeField] private EmotionId _emotion;

        public string TextId => _textId;
        public string CharacterId => _characterId;
        public EmotionId Emotion => _emotion;
    }

    public enum EmotionId
    {
        Normal,
        Happy,
        Sad,
        Angry,
        Sceared
    }
}