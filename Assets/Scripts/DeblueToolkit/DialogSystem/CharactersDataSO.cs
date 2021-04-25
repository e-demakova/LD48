using UnityEngine;

namespace Deblue.DialogSystem
{
    [CreateAssetMenu(fileName = "CharactersData", menuName = "Dialog system/Characters")]
    public class CharactersDataSO : ScriptableObject
    {
        [System.Serializable]
        public struct Character 
        {
            public CharacterID CharacterID;
            public Sprite      Icon;
            public bool        IsPlayer;
        }

        public Character[] Characters;

        public Character GetData(CharacterID id)
        {
            for (int i = 0; i < Characters.Length; i++)
            {
                if(Characters[i].CharacterID == id)
                {
                    return Characters[i];
                }
            }
            throw new System.Exception(string.Format("Character id {0} didn't register.", id));
        }
    }
}