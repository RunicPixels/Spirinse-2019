using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;

public class EnemySounds : MonoBehaviour
{
    // Note to self : make a dictionary out of all the sounds.
    
    private Rigidbody2D cachedRB;
    private BaseEnemy enemy;
    
    [EventRef] public string damageSoundEvent;
    [EventRef] public string dieSoundEvent;
    [EventRef] public string attackSoundEvent;
    [EventRef] public string idleSoundEvent;

    private EventInstance _damageInstance;
    private EventInstance _dieInstance;
    private EventInstance _attackInstance;
    private EventInstance _idleInstance;
    
    
    
    // Start is called before the first frame update
    private void Start()
    {
        if(!enemy) enemy = GetComponent<BaseEnemy>();
        cachedRB = enemy.GetComponentInChildren<Rigidbody2D>();
        
        Init();
    }

    public void OnEnable()
    {
        if(!enemy) enemy = GetComponent<BaseEnemy>();
        enemy.AttackAction += CallAttackSound;
        enemy.TakeDamageAction += CallDamageSound;
        enemy.DieAction += CallDieSound;

        Init();
    }

    public void OnDisable()
    {
        if (enemy.AttackAction != null) enemy.AttackAction -= CallAttackSound;
        if (enemy.TakeDamageAction != null)enemy.TakeDamageAction -= CallDamageSound;
        if (enemy.DieAction != null)enemy.DieAction -= CallDieSound;
    }
    
    // Update is called once per frame
    private void Update()
    {
        SetCachePosition(_damageInstance);
        SetCachePosition(_dieInstance);
        SetCachePosition(_attackInstance);
        SetCachePosition(_idleInstance);
    }

    private void Init()
    {
        _damageInstance = FMODUnity.RuntimeManager.CreateInstance(damageSoundEvent);
        _dieInstance = FMODUnity.RuntimeManager.CreateInstance(dieSoundEvent);
        _attackInstance = FMODUnity.RuntimeManager.CreateInstance(attackSoundEvent);
        _idleInstance = FMODUnity.RuntimeManager.CreateInstance(idleSoundEvent);
        
        CallIdleSound();
        
        
    }

    private void SetCachePosition(EventInstance instance)
    {
        instance.set3DAttributes(
            FMODUnity.RuntimeUtils.To3DAttributes(
            enemy.transform.gameObject,
            cachedRB));
    }

    public void CallDamageSound()
    {
        PlaySound(_damageInstance);
    }

    public void CallDieSound()
    {
        PlaySound(_dieInstance);    
    }

    public void CallAttackSound()
    {
        PlaySound(_attackInstance);    
    }

    public void CallIdleSound()
    {
        PlaySound(_idleInstance);
    }

    public void PlaySound(EventInstance instance)
    {
        instance.start();
    }

    public void OnDestroy()
    {
        StopSound(_damageInstance);
        //StopSound(_dieInstance);
        StopSound(_attackInstance);
        StopSound(_idleInstance);
    }

    public void StopSound(EventInstance instance)
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
