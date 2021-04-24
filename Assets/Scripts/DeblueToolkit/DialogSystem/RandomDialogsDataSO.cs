using System.Collections.Generic;
using UnityEngine;

namespace Deblue.DialogSystem
{
    [CreateAssetMenu(fileName = "Random_Dialogs", menuName = "Dialog system/Random dialogs")]
    public class RandomDialogsDataSO : ScriptableObject
    {
        public bool IsLastDialog => _availableDialogs.Count == 1;

        [SerializeField] private DialogSO[] _dialogs;

        [System.NonSerialized] private List<DialogSO> _availableDialogs = new List<DialogSO>(20);

        public void Init()
        {
#if UNITY_EDITOR
            _availableDialogs = new List<DialogSO>(_dialogs.Length);
#endif
            _availableDialogs.AddRange(_dialogs);
        }

        public bool GetRandomDialog(out DialogSO dialog)
        {
            dialog = null;
            if (_availableDialogs.Count > 0)
            {
                var index = Random.Range(0, _availableDialogs.Count);
                dialog = _availableDialogs[index];
                _availableDialogs.RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}