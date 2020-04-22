using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlanetHeart : MonoBehaviour
{
    public CinemachineVirtualCamera feedingCam;
    public Planet planetParent;
    public Transform planetEaterTarget;
    public void FeedToPlanetEater() {
        GetComponent<Collider>().enabled = false;
        feedingCam.m_LookAt = transform;
        StartCoroutine(feedToPE());
    }

    public AnimationCurve feedToPECurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float feedToPETime = 1f;
    private IEnumerator feedToPE() {
        feedingCam.gameObject.SetActive(true);
        float currTime = 0f;
        float lerpVal;
        Vector3 iPos = transform.position;
        WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        while (currTime < feedToPETime) {
            currTime += Time.deltaTime;
            lerpVal = feedToPECurve.Evaluate(Mathf.InverseLerp(0f, feedToPETime, currTime));
            transform.position = Vector3.LerpUnclamped(iPos, planetEaterTarget.position, lerpVal);
            yield return wfeof;
        }
        feedingCam.gameObject.SetActive(false);
        GameObject.DestroyImmediate(gameObject);
    }
}
