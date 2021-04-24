using UnityEngine;

namespace Deblue
{
    [DefaultExecutionOrder(-990)]
    public class MainCamera : UniqMono<MainCamera>
    {
        public static Camera Camera;

        protected override void MyAwake()
        {
            Camera = Camera.main;
        }
    }
}
