using Spirinse.Interfaces;
using UnityEngine;

namespace Spirinse.Objects
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseBreakableObject : MonoBehaviour, IBreakable
    {
        private Rigidbody2D rb;
        private void Awake()
        {
            foreach (Rigidbody2D rb in GetComponentsInChildren<Rigidbody2D>())
            {
                rb.isKinematic = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.GetComponent<BaseGrabObject>() && col.tag == "Throwable")
            {
                Break();
            }

        }

        public void Break()
        {
            foreach (Rigidbody2D rb in GetComponentsInChildren<Rigidbody2D>())
            {
                rb.isKinematic = false;
            }
            //Invoke("Destroy", 1f);
        }
        public void Destroy()
        {
            //Destroy(gameObject);
        }
    }
}