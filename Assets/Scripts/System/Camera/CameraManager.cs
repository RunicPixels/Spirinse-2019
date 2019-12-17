using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spirinse.System
{
    public class CameraManager : MonoBehaviour
    {
        private static CameraManager instance = null;
        public static CameraManager Instance => instance;

        public SetCameraDistance cameraDistance;
        public FollowPlayer followPlayer;
        
        private void Awake()
        {
            instance = this;
        }
    }
}
