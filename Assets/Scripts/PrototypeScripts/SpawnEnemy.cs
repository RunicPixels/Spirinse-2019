using System.Collections;
using System.Collections.Generic;
using MEC;
using Spirinse.System;
using Spirinse.Player;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    //public Cleansinator cleansinator;
    public float delay = 2f;

    public static int enemyAmount;
    public int maxEnemies = 3;

    private Vector2 spawnPosition;
    // Start is called before the first frame update
    void Start()
    {

        float velocity = Spirinse.Player.Player.Instance.GetPlayerVelocity();
        
        spawnPosition = transform.position;
        //cleansinator = CleanseManager.Instance.cleansinator;
    }

    private void OnEnable()
    {
        spawnPosition = transform.position;
        Timing.RunCoroutine(_SpawnEnemies().CancelWith(gameObject));
    }

    private IEnumerator<float> _SpawnEnemies()
    {
        while (gameObject.activeSelf)
        {
            if (enemyAmount >= maxEnemies)
            {
                yield return Timing.WaitForSeconds(0.25f);
            }
            else
            {
                enemyAmount += 1;
                //var newPos = cleansinator.GetNextObjectTransform().position;
                transform.position = spawnPosition + (Random.insideUnitCircle).normalized * Random.Range(25f, 30f);
                transform.position = new Vector3(transform.position.x, Mathf.Abs(transform.position.y), transform.position.z);
                GameObject enemy = Instantiate(enemyPrefab, transform);
                enemy.transform.position = transform.position;
                enemy.transform.parent = null;
                delay *= 0.995f;
            }
            yield return Timing.WaitForSeconds(delay);
        }
    }
}
