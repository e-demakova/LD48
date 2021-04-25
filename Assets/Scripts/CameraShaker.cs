using UnityEngine;
using Cinemachine;
using System.Collections;

namespace Deblue.LD48
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraShaker : UniqMono<CameraShaker>
    {
        public float Intensity = 0.7f;
        public float ShakingTime = 0.3f;

        private CinemachineBasicMultiChannelPerlin _camera;
        private Coroutine _shakingCoroutine;

        protected override void MyAwake()
        {
            _camera = GetComponent<CinemachineVirtualCamera>().
                      GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            _camera.m_AmplitudeGain = 0f;
        }

        public void StartShake()
        {
            StopShake();
            _shakingCoroutine = StartCoroutine(Shaking());
        }
        
        public void StopShake()
        {
            if (_shakingCoroutine != null)
            {
                StopCoroutine(_shakingCoroutine);
            }
        }

        private IEnumerator Shaking()
        {
            _camera.m_AmplitudeGain = Intensity;
            yield return new WaitForSeconds(ShakingTime);
            _camera.m_AmplitudeGain = 0f;
        }
    }
}