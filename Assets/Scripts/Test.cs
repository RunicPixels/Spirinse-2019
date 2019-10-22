using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [Clickable]
    public void DoSomething()
    {
        Debug.Log("Clicked!");
    }
}
