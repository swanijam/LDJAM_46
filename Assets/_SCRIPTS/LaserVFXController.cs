using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserVFXController : MonoBehaviour
{
    [Header("Elements")]
    public Transform beamParent;
    public Transform chargeSphere;
    public Material dishMaterial;
    public GameObject particleSystems;
    
    [Header("Animation Settings")]
    public float sphereChargeAnimLength;
    public float beamAnimLength;
    public AnimationCurve sphereChargeAnimCurve;
    public AnimationCurve dishAnimCurve;
    public AnimationCurve beamAnimCurve;
    public Animator eyeAnimController;

    public float fullBeamWidth;
    private float dishMaskVal = 0;

    public void Start()
    {
        StartEyeLaserSequence();
    }

    public void StartEyeLaserSequence()
    {
        eyeAnimController.SetBool("Laser", true);
        ChargeLaser();
    }

    public IEnumerator ChargeRoutine()
    {
        dishMaterial.SetFloat("_OuterRadius", 0);
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
            dishMaterial.SetFloat("_OuterRadius", dishAnimCurve.Evaluate(animTime));
            float newScaleVal = Mathf.Lerp(0, fullBeamWidth, beamAnimCurve.Evaluate(animTime));
            beamParent.localScale = new Vector3(newScaleVal, newScaleVal, 1);
            yield return null;
        }
        eyeAnimController.SetBool("Laser", false);
    }

    public void ChargeLaser()
    {
        particleSystems.SetActive(true);
        StartCoroutine(ChargeRoutine());
    }
    
    public void FireLaser()
    {
        StartCoroutine(FireRoutine());
    }



}
