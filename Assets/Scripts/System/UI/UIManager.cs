using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spirinse.System.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public SetHealthUI GetHealthUI => setHealthUI;

        [SerializeField] private SetHealthUI setHealthUI;

        public SetShieldUI GetShieldUI => setShieldUI;
        [SerializeField] private SetShieldUI setShieldUI;

        // Start is called before the first frame update
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

        }

    }
}
