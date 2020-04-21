using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jetpackThrustAudio : MonoBehaviour
{
    public JetpackThruster[] thrusters;
    public AudioSource thrustSound;
    public float thrustMax=.5f, thrustMod=1f;
    // Update is called once per frame
    float curVolume=0f, curVolumeVelocity=0f;
    void Update()
    {
        float thrusts = 0f;
        foreach (JetpackThruster t in thrusters) {
            if (t.firing) thrusts ++;
        }

        float targetVolume = thrusts/((float)thrusters.Length) * thrustMax;
        curVolume = Mathf.SmoothDamp(curVolume, targetVolume, ref curVolumeVelocity, .04f, 4f);
        thrustSound.volume = curVolume * thrustMod;
    }
}
