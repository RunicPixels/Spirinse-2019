using System.Collections;
using System.Collections.Generic;
using MEC;
using Spirinse.Interfaces;
using Spirinse.System;
using Spirinse.Player;
using UnityEngine;
using FMOD;
using FMODUnity;
using Spirinse.System.Effects;

public class SpawnEnemy : MonoBehaviour, IDamagable
{
    public GameObject enemyPrefab;
    //public Cleansinator cleansinator;
    public float delay = 2f;

    public StudioEventEmitter emitter;
    public List<PrototypeEnemy> enemies;
    
    public int enemyAmount;
    public int maxEnemies = 3;
    private float waitTimeAfterHit = 0f;
    public int health = 3;
    public ParticleSystem hitParticles;
    CoroutineHandle enume;
    private Vector2 spawnPosition;
    public GameObject winObject;
    // Start is called before the first frame update
    void Start()
    {
        enemyAmount = 0;
        float velocity = Spirinse.Player.Player.Instance.GetPlayerVelocity();
        
        spawnPosition = transform.position;
        //cleansinator = CleanseManager.Instance.cleansinator;
    }

    private void OnEnable()
    {
        spawnPosition = transform.position;
        enume = Timing.RunCoroutine(_SpawnEnemies().CancelWith(gameObject));
    }

    private IEnumerator<float> _SpawnEnemies()
    {
        while (gameObject.activeSelf)
        {
            if (enemyAmount >= maxEnemies || waitTimeAfterHit > 0f)
            {
                waitTimeAfterHit -= 0.25111f;
                yield return Timing.WaitForSeconds(0.25f);
            }
            else
            {
                enemyAmount += 1;
                //var newPos = cleansinator.GetNextObjectTransform().position;
                ChangePosition();
                delay *= 0.995f;
                GameObject enemy = Instantiate(enemyPrefab, transform);
                enemies.Add(enemy.GetComponent<PrototypeEnemy>());
                enemy.transform.position = transform.position;
                enemy.transform.parent = null;
            }
            yield return Timing.WaitForSeconds(delay);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonoBehaviour[] list = collision.gameObject.GetComponents<MonoBehaviour>();

        foreach (var mb in list)
        {
            if (mb is IAttack)
            {
                IAttack attack = (IAttack)mb;

                TakeDamage(attack.DoAttack());
            }
        }
    }
    
    public void ChangePosition()
    {
        transform.position = spawnPosition + (Random.insideUnitCircle).normalized * Random.Range(25f, 30f);
        transform.position = new Vector3(transform.position.x, Mathf.Abs(transform.position.y), transform.position.z);
    }
    
    public bool TakeDamage(int damage)
    {
        health -= 1;
        EffectsManager.Instance.timeManager.Freeze(0.1f, 0, 3f, 3f);
        waitTimeAfterHit = 0.5f;
        hitParticles.Play();
        ChangePosition();
        emitter.Play();
        if (health < 0)
        {
            foreach (var enemy in enemies)
            {
                enemy.TakeDamage(999);
                
                Destroy(gameObject);
                winObject.SetActive(true);
            }
        }
        
        return true;
    }

}
