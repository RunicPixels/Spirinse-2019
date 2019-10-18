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
        if (Instance != null) Instance = this;
        else Destroy(gameObject);

        if (!uIManager) uIManager = UIManager.Instance;
        if (!healthManager) healthManager = HealthManager.Instance;

        healthManager.ChangeHealthEvent += uIManager.GetHealthUI.ChangeCurrentHealth;
        healthManager.ChangeMaxHealthEvent += uIManager.GetHealthUI.ChangeMaxHealth;
    }

    private void Update()
    {
        healthManager.ChangeHealthEvent.Invoke(5);
        healthManager.ChangeMaxHealthEvent.Invoke(5);
    }

    private void OnEnable()
    {

    }

}
