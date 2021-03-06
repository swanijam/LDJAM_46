﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    public Planet myplanet;
    // public Vector3 finalScaleAfterHit = Vector3.forward;
    public Animator HeartAnimator;
     float shatterDelay = 1f;
    public void TakeHit() {
        myplanet.TakeDamage();
        // StartCoroutine(WeakPointScale());
        StartCoroutine(DelayShatter());
        GetComponent<Collider>().enabled = false;
    }

    IEnumerator DelayShatter() {
        yield return new WaitForSeconds(shatterDelay);
        HeartAnimator.SetTrigger("Shatter");
    }

    // public AnimationCurve WeakPointScaleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    // public float WeakPointScaleTime = 1f;
    // private IEnumerator WeakPointScale() {
    //     float currTime = 0f;
    //     float lerpVal;
    //     WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
    //     Vector3 iscale = transform.localScale;
    //     while (currTime < WeakPointScaleTime) {
    //         currTime += Time.deltaTime;
    //         lerpVal = WeakPointScaleCurve.Evaluate(Mathf.InverseLerp(0f, WeakPointScaleTime, currTime));
    //         transform.localScale = Vector3.Lerp(iscale, finalScaleAfterHit, lerpVal);
    //         yield return wfeof;
    //     }
    // }
}
