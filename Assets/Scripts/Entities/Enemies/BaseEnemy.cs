using System;
using System.Collections;
using System.Collections.Generic;
using RewiredConsts;
using UnityEngine;
using Spirinse.System;
using Spirinse.System.Effects;
using Spirinse.Interfaces;

public class BaseEnemy : MonoBehaviour
{
    public System.Action TakeDamageAction;
    public System.Action DieAction;
    public System.Action AttackAction;
    
    public EntityHealth health;
    public BaseEntityVisuals visuals;
    
    public enum EnemyState
    {
        Dashing,
        Idle,
        Moving,
        PreDash
    }
}