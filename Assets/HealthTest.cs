using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTest : MonoBehaviour
{
    public int beginHealth = 30;
    
    private int health;
    
    // Start is called before the first frame update
    void Start()
    {
        health = beginHealth;
        Debug.Log("Health is: " + health);
    }
    
}
