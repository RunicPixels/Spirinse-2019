using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MEC;

namespace Spirinse.System.Effects
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance;

        [SerializeField] private float playerHitFreezeDuration;

        private bool timeFreeze = false;
        private float currentTimeScale = 1f;

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


        public void SetTime(float scale)
        {
            
        }

        public void PlayerHitFreeze()
        {
            
        }

        public void FreezeCustom(float duration)
        {

        }

        public void TimeUpdate()
        {
            if (timeFreeze) return;
            Time.timeScale = currentTimeScale;
        }

        public IEnumerator<float> DoFreeze(float duration) {
            timeFreeze = true;
            while(duration > 0)
            {
                duration -= Time.unscaledDeltaTime;
                Time.timeScale = 0f;
                yield return 0f;
            }
            timeFreeze = false;
        }
    }
}
