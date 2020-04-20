﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Planet : MonoBehaviour
{
    public int health = 3;
    public Cinemachine.CinemachineImpulseSource cinemachineImpulseSource;
    public PeriodicallyLaunchObject[] plos;
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(IdleSpin());
        plos = GetComponentsInChildren<PeriodicallyLaunchObject>();
        foreach (PeriodicallyLaunchObject plo in plos) {
            if (plo != null) {
                plo.parentPlanet = this;
            }
        }
    }
    public float  idleSpinSmoothTime = 2f, idleSpinMaxSpeed = 60f;
    public float curTargetIdleSpin = 60f;
    float idleSpinVelocity, curSpinSpeed;
    IEnumerator IdleSpin() {
        curSpinSpeed = 0f;
        idleSpinVelocity = 0f;
        while (true) {
            curSpinSpeed = Mathf.SmoothDamp(curSpinSpeed, curTargetIdleSpin, ref idleSpinVelocity, idleSpinSmoothTime, 15f);
            transform.rotation *= Quaternion.AngleAxis(curSpinSpeed * Time.deltaTime, transform.up);
            yield return new WaitForEndOfFrame();
        }
    }

    public void StartCombat() {
        StopAllCoroutines();
        StartCoroutine(CombatSpin());
    }

    public AnimationCurve IdleSpinCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float IdleSpinTime = 5f;
    float combatSpinMod = 0f, combatSpinModVelocity = 0f;
    IEnumerator CombatSpin() {
        combatSpinMod = 0f;
        combatSpinModVelocity = 0f;
        SetLaunchersEnabled(true);
        while (true) { // this gets interupted by taking harm, other fight phases, etc.
            float angle = Random.Range(30f, 90f);
            Quaternion irot = transform.rotation;
            Quaternion targetOrientation = transform.rotation * Quaternion.AngleAxis(angle, (new Vector3(Random.Range(-1f, 1f), Random.Range(-.5f, .5f), Random.Range(-1f, 1f))));
            float currTime = 0f;
            float lerpVal;
            WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
            float totalTime = angle/90f * IdleSpinTime;
            while (currTime < totalTime) {
                combatSpinMod = Mathf.SmoothDamp(combatSpinMod, 1f, ref combatSpinModVelocity, .5f, 1f);
                currTime += Time.deltaTime * combatSpinMod;
                lerpVal = IdleSpinCurve.Evaluate(Mathf.InverseLerp(0f, totalTime, currTime));
                transform.rotation = Quaternion.Lerp(irot, targetOrientation, lerpVal);
                yield return wfeof;
            }
        }
    }

    public void TakeDamage() {
        health--;
        SetLaunchersEnabled(false);
        if (health <= 0 && onHealthZero != null) onHealthZero();
        StopAllCoroutines();
        StartCoroutine(Flinch());
    }

    public void SetLaunchersEnabled(bool _enabled) {
        foreach(PeriodicallyLaunchObject plo in plos) {
            if (plo != null) plo.firing = _enabled;
        }
    }

    public AnimationCurve FlinchCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float flinchFreeze = 1f;
    public float planetShakeAmplitude = .2f;
    public float FlinchTime = 1.5f;
    IEnumerator Flinch() {
        WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        cinemachineImpulseSource.GenerateImpulse();
        float currTime = 0f;
        Vector3 iPos = transform.position;
        while (currTime < flinchFreeze) {
            transform.position = iPos + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f))*planetShakeAmplitude;
            currTime += Time.deltaTime;
            yield return wfeof;
        }

        float angle = Random.Range(70f, 90f);
        Quaternion irot = transform.rotation;
        Quaternion targetOrientation = transform.rotation * Quaternion.AngleAxis(angle, new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
        float lerpVal;
        currTime = 0f;
        float totalTime = angle/30f * FlinchTime;
        while (currTime < totalTime) {
            currTime += Time.deltaTime;
            lerpVal = FlinchCurve.Evaluate(Mathf.InverseLerp(0f, totalTime, currTime));
            transform.rotation = Quaternion.Lerp(irot, targetOrientation, lerpVal);
            yield return wfeof;
        }
        if (health > 0 ) StartCoroutine(CombatSpin());
    }

    public delegate void HealthReducedToZero();
    public event HealthReducedToZero onHealthZero;

    public LayerMask playerLayer;
    private void OnTriggerEnter(Collider other)
    {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer))) {
            PlayerControllerController pcc = other.gameObject.GetComponent<PlayerControllerController>();
            if (pcc == null) Debug.LogError("not a player", other.gameObject);
            pcc.BeginOrbitCombat(this);
        }
    }
    
    // private void OnTriggerExit(Collider other)
    // {
    //     if (playerLayer == (playerLayer | (1 << other.gameObject.layer))) {
    //         PlayerControllerController pcc = other.gameObject.GetComponent<PlayerControllerController>();
    //         // SetLaunchersEnabled(false);
    //         // StopAllCoroutines();
    //         // curTargetIdleSpin = idleSpinMaxSpeed;
    //         // StartCoroutine(IdleSpin());
    //         StopAttacking();
    //     }
    // }
    public void StopAttacking() {
        SetLaunchersEnabled(false);
        StopAllCoroutines();
        curTargetIdleSpin = idleSpinMaxSpeed;
        StartCoroutine(IdleSpin());
    }
}