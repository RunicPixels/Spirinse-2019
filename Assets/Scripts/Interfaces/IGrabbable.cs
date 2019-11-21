using UnityEngine;

namespace Spirinse.Interfaces
{
    interface IGrabbable
    {
        void Hold(Transform newParent);
        void Release(Vector3 velocity);
    }
}
