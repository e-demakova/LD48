using System;

using UnityEngine;

namespace Deblue.DialogSystem
{
    [CreateAssetMenu(fileName = "New_Dialog", menuName = "Dialog system/Dialog")]
    public class DialogSO : ScriptableObject
    {
        [SerializeField] protected Replica[] _elements;

        [NonSerialized] protected int _elementIndex = -1;

        public void Init()
        {
            _elementIndex = -1;
        }

        public bool TrySwitchToNextReplica(out Replica replica)
        {
            replica = null;
            if (ElementsEnded())
            {
                return false;
            }
            _elementIndex++;
            replica = _elements[_elementIndex];
            return true;
        }

        public bool ElementsEnded()
        {
            return _elementIndex >= _elements.Length - 1;
        }
    }
}