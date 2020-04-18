using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Planet : MonoBehaviour
{
    public int health = 3;
    public CinemachineImpulseSource cinemachineImpulseSource;
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(IdleSpin());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public AnimationCurve IdleSpinCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float IdleSpinTime = 5f;
    IEnumerator IdleSpin() {
        while (true) { // this gets interupted by taking harm, other fight phases, etc.
            float angle = Random.Range(15f, 90f);
            Quaternion irot = transform.rotation;
            Quaternion targetOrientation = transform.rotation * Quaternion.AngleAxis(angle, new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
            float currTime = 0f;
            float lerpVal;
            WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
            float totalTime = angle/90f * IdleSpinTime;
            while (currTime < totalTime) {
                currTime += Time.deltaTime;
                lerpVal = IdleSpinCurve.Evaluate(Mathf.InverseLerp(0f, totalTime, currTime));
                transform.rotation = Quaternion.Lerp(irot, targetOrientation, lerpVal);
                yield return wfeof;
            }
        }
    }

    public void TakeDamage() {
        health--;
        if (health <= 0 && onHealthZero != null) onHealthZero();
        StopAllCoroutines();
        StartCoroutine(Flinch());
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
        StartCoroutine(IdleSpin());
    }

    public delegate void HealthReducedToZero();
    public event HealthReducedToZero onHealthZero;
}
