using UnityEngine;

namespace Deblue
{
    public class UniqMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        protected void Awake()
        {
            CheckIsUniq();
            TrySetInstanceOrDestroy();
            MyAwake();
        }

        protected virtual void MyAwake()
        {
        }

        protected void CheckIsUniq()
        {
#if UNITY_EDITOR
            var type = FindObjectsOfType(typeof(T));
            if (type.Length > 1)
            {
                Debug.LogWarningFormat("There is more than one {0} in the scene. " +
                "Please ensure there is always exactly one {0} in the scene.", typeof(T).Name);
            }
#endif
        }

        protected void TrySetInstanceOrDestroy()
        {
            if (_instance == null)
            {
                _instance = gameObject.GetComponent<T>();
            }
            else if (_instance != this)
            {
                Destroy(this);
            }
        }

        protected static bool TryGetInstance(out T instance)
        {
            instance = _instance;
            return _instance != null;
        }
    }
}
