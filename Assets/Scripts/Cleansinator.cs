using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cleansinator : MonoBehaviour
{
    // VERY BASIC SETUP FOR CLEANSING WORLD;

    public Material cleansedMaterial;

    public List<MeshRenderer> objectsToCleanse;

    public int currentNumber = 0;
    public int currentEnemyNumber = 0;

    public void Start()
    {
        currentNumber = 0;
        currentEnemyNumber = 0;
    }

    public void CleanseNextObject()
    {
        if (currentNumber > objectsToCleanse.Count) SceneManager.LoadScene(0);
        //objectsToCleanse[currentNumber].material = cleansedMaterial;
        currentNumber += 1;
    }
    public void ResetCleansinator()
    {
        currentNumber = 0;
    }
    public Transform GetNextObjectTransform()
    {
        currentEnemyNumber += 1;
        return objectsToCleanse[currentEnemyNumber].transform;
    }
}
