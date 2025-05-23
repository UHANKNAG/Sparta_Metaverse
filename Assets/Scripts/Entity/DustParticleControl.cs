using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustParticleControl : MonoBehaviour
{
    [SerializeField] private bool createDustOnWalk = true;
    [SerializeField] private ParticleSystem dustParticleSystem;
    
    public void CreateDustOnWalk() {
        if (createDustOnWalk) {
            dustParticleSystem.Stop();
            dustParticleSystem.Play();
        }
    }
}
