using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    private InputManager _inputManager;

    [SerializeField] protected HealthManager healthManager;
    protected SetHealthUI healthUI;


    public HealthManager HealthManager => healthManager;

    // Start is called before the first frame update
    private void Start()
    {
        if (Instance != null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (!healthManager) healthManager = HealthManager.Instance;

        healthManager.ChangeHealthEvent += healthUI.ChangeCurrentHealth;
        healthManager.ChangeMaxHealthEvent += healthUI.ChangeMaxHealth;
    }

    private void OnEnable()
    {

    }

}
