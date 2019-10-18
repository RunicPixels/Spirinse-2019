using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public SetHealthUI GetHealthUI => setHealthUI;

    [SerializeField] private SetHealthUI setHealthUI;
    // Start is called before the first frame update
    private void Start()
    {
        if (Instance != null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

}
