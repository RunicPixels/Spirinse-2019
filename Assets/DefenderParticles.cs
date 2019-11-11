using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderParticles : MonoBehaviour
{
    public List<ParticleSystem> hitParticleSystems;
    // Start is called before the first frame update
    public void GetHit(int intensity)
    {
        foreach(ParticleSystem system in hitParticleSystems)
        {
            system.Play();
        }
    }
}
