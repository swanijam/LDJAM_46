using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeVFXController : MonoBehaviour
{
    public ParticleSystem spikeParticles;

    public void PlaySpikeParticles()
    {
        spikeParticles.Play();
    }
}
