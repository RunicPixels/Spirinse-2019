using Spirinse.Interfaces;
using UnityEngine;

namespace Spirinse.Objects
{
    public class BaseBreakableObject : MonoBehaviour, IBreakable
    {
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.transform.GetComponent<BaseGrabObject>())
            {
                Break();
            }
            
        }

        public void Break()
        {
            Destroy(gameObject);
        }
    }
}