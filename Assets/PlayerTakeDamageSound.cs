using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Spirinse.Player;
using Spirinse.System;
using Spirinse.System.Health;
using UnityEngine;

public class PlayerTakeDamageSound : MonoBehaviour
{
    [EventRef] public string damageSound;

    [EventRef] public string lowHealthSound;
    
    
    private Rigidbody2D cachedRB;
    private Spirinse.Player.Player player;
    
    private FMOD.Studio.EventInstance _damageEventInstance;
    private FMOD.Studio.EventInstance _lowHealthEventInstance;
    
    
    
    private void Start()
    {
        Setup();
        //GameManager.Instance.GameStartEvent += Setup;
        HealthManager.Instance.DamageEvent += DoDamage;
        HealthManager.Instance.ChangeHealthEvent += ChangeHealth;
    }

    private void Setup()
    {
        _damageEventInstance = FMODUnity.RuntimeManager.CreateInstance(damageSound);
        _lowHealthEventInstance = FMODUnity.RuntimeManager.CreateInstance(lowHealthSound);
        cachedRB = Spirinse.Player.Player.Instance.controls.GetRB();
        player = Spirinse.Player.Player.Instance;
        
    }
    
    public void DoDamage(int damage) {
        _damageEventInstance.set3DAttributes(
            RuntimeUtils.To3DAttributes(
                player.defender.transform.gameObject,
                cachedRB));
        
        if (damage > 0)
        {
            _damageEventInstance.start();
        }
    }

    public void ChangeHealth(int newHealth)
    {
        _damageEventInstance.set3DAttributes(
            RuntimeUtils.To3DAttributes(
                player.defender.transform.gameObject,
                cachedRB));
        
        if (newHealth <= 2)
        {
            _lowHealthEventInstance.start();
        }
    }
}
