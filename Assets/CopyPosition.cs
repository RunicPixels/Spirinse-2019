using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Spirinse.Player.Player.Instance)
        {
            transform.position = Spirinse.Player.Player.Instance.defender.transform.position;
        }
    }
}
