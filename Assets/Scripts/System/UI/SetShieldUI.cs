using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Spirinse.System.UI
{
    public class SetShieldUI : MonoBehaviour
    {
        // Future Edit Notes : Maybe Inherit from SetHealthUI (Leaving it out of it right now because of volatility of implementation).
        public GameObject UIPrefab;

        [Header("Shield")] public Sprite ActiveShieldSprite;
        public Sprite InactiveShieldSprite;
        public Sprite BrokenShieldSprite;

        [Header("Inactive Parent")] public Transform inactiveParent;

        private int maxShieldContainers;
        private int currentShieldContainers;
        private int currentShield;


        private List<Image> ShieldSprites = new List<Image>();

        public void SetMaxShieldContainers(int maxShieldContainers)
        {
            this.maxShieldContainers = maxShieldContainers;
            ChangeShieldUI();
        }

        public void ChangeMaxShield(int newShieldMax)
        {
            currentShieldContainers = newShieldMax;
            UpdateShieldUI();
        }

        public void ChangeCurrentShield(int newShield)
        {
            currentShield = newShield;
            UpdateShieldUI();
        }

        private void ChangeShieldUI()
        {
            foreach (var obj in ShieldSprites) Destroy(obj.gameObject);
            ShieldSprites = new List<Image>();
            for (int i = 0; i < maxShieldContainers; i++)
            {
                GameObject cur = Instantiate(UIPrefab, transform);
                RectTransform rTransform = cur.GetComponent<RectTransform>();
                ShieldSprites.Add(cur.GetComponent<Image>());
            }

            UpdateShieldUI();
        }

        private void UpdateShieldUI()
        {
            for (int i = 0; i < maxShieldContainers; i++)
            {
                var image = ShieldSprites[i];
                if( i == currentShield - 1)
                {
                    image.sprite = ActiveShieldSprite;
                    image.transform.SetParent(transform);
                    image.gameObject.SetActive(true);
                }
                else if (i < currentShield)
                {
                    image.sprite = InactiveShieldSprite;
                    image.transform.SetParent(transform);
                    image.gameObject.SetActive(true);
                }

                else if (i < currentShieldContainers)
                {
                    image.sprite = BrokenShieldSprite;
                    image.transform.SetParent(transform);
                    image.gameObject.SetActive(true);
                }

                else if (i >= currentShieldContainers)
                {
                    image.sprite = null;
                    image.transform.SetParent(inactiveParent);
                    image.gameObject.SetActive(false);
                }
            }
        }
    }
}