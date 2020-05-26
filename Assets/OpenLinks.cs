using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLinks : MonoBehaviour
{
    public string link;
    // Start is called before the first frame update

    public void OpenLink() {
        Application.OpenURL(link);
    }
}
