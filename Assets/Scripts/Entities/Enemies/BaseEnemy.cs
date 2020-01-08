using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spirinse.System;
using Spirinse.System.Effects;
using Spirinse.Interfaces;

public class BaseEnemy : MonoBehaviour
{
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