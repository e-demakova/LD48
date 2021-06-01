using UnityEngine;

namespace Deblue
{
    public class PersistentMono<T> : UniqMono<T> where T : MonoBehaviour
    {
        new protected void Awake()
        {
            CheckIsUniq();
            DontDestroyOnLoad(this);
            MyAwake();
        }
    }
}