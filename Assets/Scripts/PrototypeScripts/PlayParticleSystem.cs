using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class PlayParticleSystem : MonoBehaviour
{
    
    // Start is called before the first frame update
    private ParticleSystem system;
    void Start()
    {
        system = GetComponent<ParticleSystem>();
        Burst();
    }

    public void Burst()
    {
        system.Play();
    }
}
