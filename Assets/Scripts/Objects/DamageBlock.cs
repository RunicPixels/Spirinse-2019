using System.Collections;
using System.Collections.Generic;
using Spirinse.Interfaces;
using UnityEngine;

public class DamageBlock : MonoBehaviour
{

void OnCollisionEnter2D(Collision2D collision)
{
    IDamagable damageable = collision.gameObject.GetComponent<IDamagable>();
    if(damageable  != null)
    {
      damageable.TakeDamage(1);
    }
  }

  void OnCollisionStay2D(Collision2D collision)
  {
      IDamagable damageable = collision.gameObject.GetComponent<IDamagable>();
      if(damageable  != null)
      {
        damageable.TakeDamage(1);
      }
    }
}
