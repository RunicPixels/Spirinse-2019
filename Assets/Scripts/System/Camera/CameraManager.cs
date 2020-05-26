using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            Setup();

        }

        public void Setup()
        {
            if (!cameraDistance) cameraDistance = FindObjectOfType<SetCameraDistance>();
            if (!followPlayer) followPlayer = FindObjectOfType<FollowPlayer>();
        }
    }
}
