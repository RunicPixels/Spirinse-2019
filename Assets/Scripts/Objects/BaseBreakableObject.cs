using Spirinse.Interfaces;
using UnityEngine;

namespace Spirinse.Objects
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseBreakableObject : MonoBehaviour, IBreakable
    {
        [EnumFlag][SerializeField]
        System.Enums.DamageEnums.DamageType damageTypes = System.Enums.DamageEnums.DamageType.Everything;

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
            var dt = col.GetComponent<Spirinse.System.Combat.DamageType>();
            if (dt != null)
            {
                if(damageTypes.HasFlag(dt.damageType)) Break();
            }

        }

        public void Break()
        {
            foreach (Rigidbody2D rb in GetComponentsInChildren<Rigidbody2D>())
            {
                rb.isKinematic = false;
            }
            Destroy();
        }
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}