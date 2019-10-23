using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Spirinse.System.UI
{
    public class SetHealthUI : MonoBehaviour
    {
        public GameObject UIPrefab;

        [Header("Health")] public Sprite FullHealthSprite;
        public Sprite EmptyHeartSprite;


        [Header("Inactive Parent")] public Transform inactiveParent;

        private int maxHealthContainers;
        private int currentHealthContainers;
        private int currentHealth;

        private List<Image> HealthSprites = new List<Image>();

        public void SetMaxHealthContainers(int maxHPContainers)
        {
            maxHealthContainers = maxHPContainers;
            ChangeHealthUI();
        }

        public void ChangeMaxHealth(int newHealthMax)
        {
            currentHealthContainers = newHealthMax;
            UpdateHealthUI();
        }

        public void ChangeCurrentHealth(int newHealth)
        {
            currentHealth = newHealth;
            UpdateHealthUI();
        }

        private void ChangeHealthUI()
        {
            foreach (var obj in HealthSprites) Destroy(obj.gameObject);
            HealthSprites = new List<Image>();
            for (int i = 0; i < maxHealthContainers; i++)
            {
                GameObject cur = Instantiate(UIPrefab, transform);
                RectTransform rTransform = cur.GetComponent<RectTransform>();
                HealthSprites.Add(cur.GetComponent<Image>());
            }

            UpdateHealthUI();
        }

        private void UpdateHealthUI()
        {
            for (int i = 0; i < maxHealthContainers; i++)
            {
                var image = HealthSprites[i];
                if (i < currentHealth)
                {
                    image.sprite = FullHealthSprite;
                    image.transform.SetParent(transform);
                    image.gameObject.SetActive(true);
                }

                else if (i < currentHealthContainers)
                {
                    image.sprite = EmptyHeartSprite;
                    image.transform.SetParent(transform);
                    image.gameObject.SetActive(true);
                }

                else if (i >= currentHealthContainers)
                {
                    image.sprite = null;
                    image.transform.SetParent(inactiveParent);
                    image.gameObject.SetActive(false);
                }
            }
        }
    }
}