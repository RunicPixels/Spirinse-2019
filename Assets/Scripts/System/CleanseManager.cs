using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Spirinse.System
{
    public class CleanseManager : MonoBehaviour
    {
        public static CleanseManager Instance;
        public Cleansinator cleansinator;
        public Action cleanseEvent;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
            cleanseEvent += cleansinator.CleanseNextObject;
        }
    }
}
