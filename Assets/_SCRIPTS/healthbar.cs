using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthbar : MonoBehaviour
{
    public float _health;
    public float health {
        get {
            return _health;
        }
        set {
            _health = value;
            UpdateHealthBarScale();
        }
    }
    
    public Transform healthTarget;
    public Vector3 scaleMin, scaleMax;
    public void UpdateHealthBarScale() {
        healthTarget.localScale = Vector3.LerpUnclamped(scaleMin, scaleMax, Mathf.InverseLerp(varRange.x, varRange.y, health));
    }
    public void FadeToVal(float val) {
        StartCoroutine(_FadeToVal(val));
    }
    private IEnumerator _FadeToVal(float val) {
        float currTime = 0f;
        float ival = health;
        while (currTime < time) {
            currTime += Time.deltaTime;
            float lerpVal = curve.Evaluate(Mathf.InverseLerp(0f, time, currTime));
            //r.material.SetFloat(propertyName, Mathf.LerpUnclamped(varRange.y, varRange.x, lerpVal));
            health = Mathf.LerpUnclamped(ival, val, lerpVal);
            yield return new WaitForEndOfFrame();
        }
    }


    public Vector2 varRange = Vector2.up; 
    public float time = 1f;
    public AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    public void FadeToMax () {
        StartCoroutine(_FadeToMax());
    }
    public void FadefloatToMax () {
        StartCoroutine(_FadeToMax());
    }

    private IEnumerator _FadeToMax() {
        float currTime = 0f;
        float ival = health;
        while (currTime < time) {
            currTime += Time.deltaTime;
            float lerpVal = curve.Evaluate(Mathf.InverseLerp(0f, time, currTime));
            //r.material.SetFloat(propertyName, Mathf.LerpUnclamped(varRange.x, varRange.y, lerpVal));
            health = Mathf.LerpUnclamped(ival, varRange.y, lerpVal);
            yield return new WaitForEndOfFrame();
        }
    }

    public void FadeToMin () {
        StartCoroutine(_FadeToMin());
    }
    public void FadefloatToMin () {
        StartCoroutine(_FadeToMin());
    }
    private IEnumerator _FadeToMin() {
        float currTime = 0f;
        float ival = health;
        while (currTime < time) {
            currTime += Time.deltaTime;
            float lerpVal = curve.Evaluate(Mathf.InverseLerp(0f, time, currTime));
            //r.material.SetFloat(propertyName, Mathf.LerpUnclamped(varRange.y, varRange.x, lerpVal));
            health = Mathf.LerpUnclamped(ival, varRange.x, lerpVal);
            yield return new WaitForEndOfFrame();
        }
    }


}
