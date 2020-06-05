using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spirinse.Interfaces;
using Spirinse.System.Combat;
using MEC;

namespace Spirinse.Objects
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseGrabObject : MonoBehaviour, IGrabbable
    {
        private Rigidbody2D rb;
        private Collider2D[] colliders;
        public float damage = 1;

        private DamageType damageType;
        private int oldLayer = 0;
        private float oldDrag = 0;
        private float oldGrav = 0;
        private float oldAngDrag = 0;
        private const float ReleaseTime = 4f;
        private bool held = false;
        
        private void Awake()
        {
            damageType = SetDamageType();

            oldLayer = gameObject.layer;
            
            
            colliders = GetComponents<Collider2D>();
            rb = GetComponent<Rigidbody2D>();
            
            oldDrag = rb.drag;
            oldGrav = rb.gravityScale;
            oldAngDrag = rb.angularDrag;

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Defender")) return;

            MonoBehaviourCollisionCheck(collision.gameObject.GetComponents<MonoBehaviour>());
        }

        public void Hold(Transform newParent)
        {
            float offset = 2f;

            foreach (Collider2D col in colliders)
            {
                if (!col.isTrigger) offset = col.bounds.size.y + 1;
                gameObject.layer = Layers.PlayerProjectile;
            }
            held = true;
            //transform.localRotation = newParent.localRotation;
            rb.drag = 1.5f;
            rb.angularDrag = 0.05f;
            rb.gravityScale = 0.6f;
            //rb.isKinematic = true;
            //transform.parent = newParent;
            //transform.localPosition = newParent.position + new Vector3(0, -offset, 0);

        }

        public void Release(Vector3 velocity)
        {
  
            held = false;
            //rb.isKinematic = false;
            // Slight Delay until collisions return;
            Timing.RunCoroutine(_ReleaseReturn());
            //dj.connectedBody = null;
            //dj.enabled = false;
            //transform.parent = null;
            rb.velocity += (Vector2)velocity;
        }

        private IEnumerator<float> _ReleaseReturn()
        {
            var drag = rb.drag;
            var angDrag = rb.angularDrag;
            var grav = rb.gravityScale;
            var t = 0f;

            while (t < 1f)
            {
                if (held) break;
                rb.drag = Mathf.Lerp(drag, oldDrag, t);
                rb.angularDrag = Mathf.Lerp(angDrag, oldAngDrag, t);
                rb.gravityScale = Mathf.Lerp(grav, oldGrav, t);
                
                t += Time.deltaTime;
                yield return 0;
            }
            
            ReturnCollision();
        }
        
        public void ReturnCollision()
        {
            foreach (Collider2D col in colliders)
            {
                gameObject.layer = oldLayer;
            }
        }

        public int GetDamage()
        {
            return Mathf.FloorToInt(damage * rb.velocity.magnitude * 0.1f);
        }

        public void MonoBehaviourCollisionCheck(MonoBehaviour[] list)
        {
            foreach (var mb in list)
            {
                if (mb is IDamagable)
                {
                    IDamagable damageable = (IDamagable)mb;
                    damageable.TakeDamage(GetDamage());
                }
            }
        }

        private DamageType SetDamageType()
        {
            var dt = GetComponent<DamageType>();
            if(dt == null)
            {
                dt = gameObject.AddComponent<DamageType>();
                dt.damageType = System.Enums.DamageEnums.DamageType.Grab;
            }
            return dt;
        }

        public Rigidbody2D GetRigidbody2D()
        {
            return rb;
        }

        public Transform GetTransform()
        {
            return transform;
        }
    }
}
