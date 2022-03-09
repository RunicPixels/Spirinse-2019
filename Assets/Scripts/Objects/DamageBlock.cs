using System.Collections;
using System.Collections.Generic;
using Spirinse.Interfaces;
using UnityEngine;

public class DamageBlock : MonoBehaviour
{
    public bool activated = true;

void OnCollisionEnter2D(Collision2D collision)
{
        if (!activated) return;
    IDamagable damageable = collision.gameObject.GetComponent<IDamagable>();
    if(damageable  != null)
    {
      damageable.TakeDamage(1);
    }
  }

  void OnCollisionStay2D(Collision2D collision)
  {
        if (!activated) return;
      IDamagable damageable = collision.gameObject.GetComponent<IDamagable>();
      if(damageable  != null)
      {
        damageable.TakeDamage(1);
      }
    }
}
