using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spirinse.System.Effects
{
    public class EffectsManager : MonoBehaviour
    {
        public static EffectsManager Instance;
        public TimeManager timeManager;
        public CameraShake cameraShake;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        private void OnDestroy()
        {
            Debug.LogWarning("Destroying" + Instance);
            Instance = null;
        }
    }
}
