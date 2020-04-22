using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserVFXController : MonoBehaviour
{
    [Header("Elements")]
    public Transform beamParent;
    public Transform chargeSphere;
    // public Material dishMaterial;
    public MeshRenderer dish;
    public GameObject particleSystems;
    public ParticleSystem systemParent;
    
    [Header("Animation Settings")]
    public float sphereChargeAnimLength;
    public float beamAnimLength;
    public AnimationCurve sphereChargeAnimCurve;
    public AnimationCurve dishAnimCurve;
    public AnimationCurve beamAnimCurve;
    public Animator eyeAnimController;
    public float fullBeamWidth;

    public bool play;
    private float dishMaskVal = 0;

    public void Start()
    {
        // Enable this for testing in editor
        // StartCoroutine(PlayLoop());
    }

    public IEnumerator PlayLoop()
    {
        while(true)
        {
            yield return new WaitUntil(() => play);
            StartEyeLaserSequence();
            play = false;
        }
    }

    public void StartEyeLaserSequence()
    {
        // systemParent.Play();
        eyeAnimController.SetBool("Laser", true);
        ChargeLaser();
    }

    public IEnumerator ChargeRoutine()
    {
        dish.materials[0].SetFloat("_OuterRadius", 0);
        float animTime = 0;
        while(animTime <= 1)
        {
            animTime += Time.deltaTime/sphereChargeAnimLength;
            chargeSphere.localScale = Vector3.one * sphereChargeAnimCurve.Evaluate(animTime);
            yield return null;
        }
        FireLaser();
    }

    public IEnumerator FireRoutine()
    {
        float animTime = 0;
        while(animTime <= 1)
        {
            animTime += Time.deltaTime/beamAnimLength;
            dish.materials[0].SetFloat("_OuterRadius", dishAnimCurve.Evaluate(animTime));
            float newScaleVal = Mathf.Lerp(0, fullBeamWidth, beamAnimCurve.Evaluate(animTime));
            beamParent.localScale = new Vector3(newScaleVal, newScaleVal, 1);
            yield return null;
        }
        eyeAnimController.SetBool("Laser", false);
    }

    public void ChargeLaser()
    {
        particleSystems.SetActive(false);
        StartCoroutine(ChargeRoutine());
        particleSystems.SetActive(true);
    }
    
    public void FireLaser()
    {
        StartCoroutine(FireRoutine());
    }



}
