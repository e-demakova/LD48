using UnityEngine;

namespace Deblue.DialogSystem
{
    [System.Serializable]
    public class Replica
    {
        [TextArea(3, 10)]
        public string    Text;
        public CharacterID Character;
    }

    public enum CharacterID
    {
        OldMan,
        Boy
    }
}