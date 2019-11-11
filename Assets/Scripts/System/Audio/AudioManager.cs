using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spirinse.System.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance = null;
        public static AudioManager Instance => instance;
        // Start is called before the first frame update
        private void Awake()
        {
            if (Instance == null) instance = this;
            else Destroy(gameObject);
        }
        private void OnDestroy()
        {
            Debug.LogWarning("Destroying" + Instance);
            instance = null;
        }
    }
}
