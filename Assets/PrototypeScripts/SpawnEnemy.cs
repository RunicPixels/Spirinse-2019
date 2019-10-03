using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;

    public float delay = 2f;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        Timing.RunCoroutine(_SpawnEnemies());
    }

    private void OnDisable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator<float> _SpawnEnemies()
    {
        while (gameObject.activeSelf)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform);
            enemy.transform.position = transform.position;
            enemy.transform.parent = null;
            yield return Timing.WaitForSeconds(delay);
        }
    }
}
