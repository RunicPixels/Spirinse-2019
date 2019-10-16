using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    private InputManager _inputManager;

    protected HealthManager healthManager;

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
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
