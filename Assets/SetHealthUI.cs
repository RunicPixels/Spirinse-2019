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

    private int maxHealthContainers;

    private int currentHealthContainers = 3;
    private int currentHealth = 3;
    private int currentShieldContainers = 3;
    

    private List<Image> HealthSprites;
    private List<Image> ShieldSprites;

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
        foreach(var obj in HealthSprites) Destroy(obj);
        HealthSprites = new List<Image>();
        for (int i = 0; i < maxHealthContainers; i++)
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
            if (i <= currentHealth) HealthSprites[i].sprite = FullHealthSprite;
            else if (i <= currentHealthContainers) HealthSprites[i].sprite = EmptyHeartSprite;
            else HealthSprites[i].sprite = null;
        }
    }
}
