using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class SetHealthUI : MonoBehaviour
{
    public GameObject UIPrefab;
    public int distance;

    [Header("Health")]
    public Sprite FullHealthSprite;
    public Sprite EmptyHeartSprite;

    [Header("Shield")]
    public Sprite ActiveShieldSprite;
    public Sprite InactiveShieldSprite;
    public Sprite BrokenShieldSprite;

    private int currentHealthContainers;
    private int currentHealth;
    private int currentShieldContainers;

    private List<Image> HealthSprites;
    private List<Image> ShieldSprites;

    public void ChangeMaxHealth(int newHealthMax)
    {
        currentHealthContainers = newHealthMax;
        ChangeHealthUI();
    }

    public void ChangeCurrentHealth(int newHealth)
    {
        currentHealth = newHealth;
        UpdateHealthUI();
    }

    private void ChangeHealthUI()
    {
        HealthSprites.Clear();
        for (int i = 0; i < currentHealthContainers; i++)
        {
            GameObject cur = Instantiate(UIPrefab);
            cur.transform.SetParent(transform);
            RectTransform rTransform = cur.GetComponent<RectTransform>();
            rTransform.anchoredPosition.Set(rTransform.anchoredPosition.x + (distance * i),rTransform.anchoredPosition.y);
            HealthSprites.Add(cur.GetComponent<Image>());
        }
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        for (int i = 0; i < currentHealthContainers; i++)
        {
            if (i <= currentHealthContainers) HealthSprites[i].sprite = FullHealthSprite;
            else HealthSprites[i].sprite = EmptyHeartSprite;
        }
    }
}
