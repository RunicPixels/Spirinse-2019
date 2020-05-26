using System.Collections;
using System.Collections.Generic;
using Spirinse.System;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public string scene = "Main";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoChangeScene(string scene)
    {
        GameManager.Instance.GameStart();
    }
}
