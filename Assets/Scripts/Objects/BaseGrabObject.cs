using UnityEngine;
using System.Collections;
using Spirinse.Interfaces;
using Spirinse.System.Combat;

namespace Spirinse.Objects
{
    [RequireComponent(typeof(Rigidbody2D))][RequireComponent(typeof(DistanceJoint2D))]
    public class BaseGrabObject : MonoBehaviour, IGrabbable
    {
        private Rigidbody2D rb;
        private DistanceJoint2D dj;
        private Collider2D[] colliders;
        public float damage = 1;

        private DamageType damageType;

        private void Awake()
        {
            damageType = SetDamageType();

            colliders = GetComponents<Collider2D>();
            rb = GetComponent<Rigidbody2D>();
            dj = GetComponent<DistanceJoint2D>();
            dj.enabled = false;
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
                col.enabled = false;
            }
            transform.localRotation = newParent.localRotation;
            rb.isKinematic = true;
            dj.enabled = true;
            dj.connectedBody = newParent.GetComponentInParent<Rigidbody2D>();
            transform.parent = newParent;
            transform.localPosition = new Vector3(0, -offset, 0);

        }

        public void Release(Vector3 velocity)
        {
            rb.isKinematic = false;
            // Slight Delay until collisions return;
            Invoke(nameof(ReturnCollision), 0.1f);
            dj.connectedBody = null;
            dj.enabled = false;
            transform.parent = null;
            rb.velocity = velocity;
        }
        public void ReturnCollision()
        {
            foreach (Collider2D col in colliders)
            {
                col.enabled = true;
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
    }
}
