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

        private void OnDestroy()
        {
            Debug.LogWarning("Destroying" + Instance);
            Instance = null;
        }


        public void SetTime(float scale)
        {
            
        }

        public void PlayerHitFreeze(int damage)
        {
            Timing.RunCoroutine(DoFreeze(playerHitFreezeDuration, 0.05f, 4f, 4f));
        }

        public void FreezeCustom(float duration)
        {
            Timing.RunCoroutine(DoFreeze(duration));
        }

        public void TimeUpdate()
        {
            if (timeFreeze) return;
            Time.timeScale = currentTimeScale;
        }

        public IEnumerator<float> DoFreeze(float duration, float delay = 0.1f, float timeDecrease = 4f, float timeIncrease = 4f) {
            timeFreeze = true;
            while(delay>0)
            {
                delay -= Time.unscaledDeltaTime;
                yield return 0f;
            }

            while(duration > 0)
            {
                if(Time.timeScale > 0) Time.timeScale -= (Mathf.Max(timeDecrease * Time.unscaledDeltaTime, Time.timeScale));
                duration -= Time.unscaledDeltaTime;
                yield return 0f;
            }
            while(Time.timeScale < currentTimeScale)
            {
                Time.timeScale += timeIncrease * Time.unscaledDeltaTime;
                yield return 0f;
            }
            Time.timeScale = currentTimeScale;
            timeFreeze = false;
        }
    }
}
