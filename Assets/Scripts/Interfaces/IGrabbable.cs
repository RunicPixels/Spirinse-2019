using UnityEngine;

namespace Spirinse.Interfaces
{
    public interface IGrabbable
    {
        void Hold(Transform newParent);
        void Release(Vector3 velocity);
        Rigidbody2D GetRigidbody2D();
        Transform GetTransform();
    }
}
