using UnityEngine;
using System.Collections;
using Spirinse.Interfaces;

namespace Spirinse.Objects
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseGrabObject : MonoBehaviour, IGrabbable
    {
        private Rigidbody2D rb;
        private Collider2D[] colliders;
        public float damage = 1;

        private void Awake()
        {
            colliders = GetComponents<Collider2D>();
            rb = GetComponent<Rigidbody2D>();
        }

        public void Hold(Transform newParent)
        {
            var offset = 0f;
            foreach(Collider2D col in colliders)
            {
                col.enabled = false;
                if (!col.isTrigger) offset = col.bounds.size.y;

            }
            
            rb.isKinematic = true;
            transform.parent = newParent;

            transform.localPosition = new Vector3(0, offset , 0);

        }

        public void Release(Vector3 velocity)
        {
            rb.isKinematic = false;
            // Slight Delay until collisions return;
            Invoke(nameof(ReturnCollision), 0.1f);
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
    }
}
