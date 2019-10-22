using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;



    [SerializeField] protected HealthManager healthManager;
    


    public HealthManager HealthManager => healthManager;

    protected UIManager uIManager;

    private InputManager _inputManager;

    // Start is called before the first frame update
    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (uIManager == null) uIManager = UIManager.Instance;
        if (healthManager == null) healthManager = HealthManager.Instance;

        uIManager.GetHealthUI.SetMaxHealthContainers(HealthManager.GetHealthCap);

        healthManager.ChangeHealthEvent += uIManager.GetHealthUI.ChangeCurrentHealth;
        healthManager.ChangeMaxHealthEvent += uIManager.GetHealthUI.ChangeMaxHealth;
    }

}
