using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePlanet : MonoBehaviour
{
    public PlanetGesture planetGesture;
    public GameObject FireBreath;
    public float fireBreathDuration;
    public float hurtDuration;
    public Planet planet;
    public bool IsBreathFirePeriodically = false;
    public bool IsBreathFire = false;

    void OnEnable()
    {
        planet.enterPlanet += BreathFirePeriodically;
        planet.onHurtPlanet += HurtGesture;
    }

    public void BreathFirePeriodically()
    {
        IsBreathFirePeriodically = true;
        StartCoroutine(BreathFirePeriodicallyAnimation());
    }

    public void HurtGesture()
    {
        StopFireBreath();
        StartCoroutine(HurtGestureAnimation());
    }


    // public void BreathFire()
    // {
    //     StartCoroutine(BreathFireAnimation());
    // }


    public void StopFireBreath()
    {
        IsBreathFirePeriodically = false;
        StopAllCoroutines();
        FireBreath.SetActive(false);
    }

    public IEnumerator HurtGestureAnimation()
    {
        planetGesture.SetFace(PlanetGesture.FaceState.HURT);
        yield return new WaitForSeconds(planetGesture.duration);
        yield return new WaitForSeconds(hurtDuration);
        BreathFirePeriodically();
    }

    public IEnumerator BreathFirePeriodicallyAnimation()
    {
        while(IsBreathFirePeriodically)
        {
            yield return new WaitForSeconds(Random.Range(2, 3));
            yield return StartCoroutine(BreathFireAnimation());
            yield return new WaitForSeconds(Random.Range(4, 8));
        }
    }

    public IEnumerator BreathFireAnimation()
    {
        planetGesture.SetFace(PlanetGesture.FaceState.INHALEFIRE);
        yield return new WaitForSeconds(planetGesture.duration);
        planetGesture.SetFace(PlanetGesture.FaceState.EXHALEFIRE);
        FireBreath.SetActive(true);
        yield return new WaitForSeconds(fireBreathDuration);
        FireBreath.SetActive(false);
        planetGesture.SetFace(PlanetGesture.FaceState.IDLE);
        yield return new WaitForSeconds(planetGesture.duration);
    }
}
