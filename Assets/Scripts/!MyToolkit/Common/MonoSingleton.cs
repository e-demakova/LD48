using UnityEngine;

namespace Deblue
{
    public class MonoSingleton<T> : UniqMono<T> where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                if (!TryGetInstance(out var instance))
                {
                    instance = (T)FindObjectOfType(typeof(T));
                    if (!TryGetInstance(out instance))
                    {
#if UNITY_EDITOR
                        Debug.LogWarningFormat($"{typeof(T).Name} not set in scene. ");
#endif
                        var newObject = new GameObject(typeof(T).ToString());
                        newObject.AddComponent<T>();
                    }
                }
                return instance;
            }

        }
    }
}