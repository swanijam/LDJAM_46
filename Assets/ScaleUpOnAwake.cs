using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUpOnAwake : MonoBehaviour
{
    Vector3 fullSize;
    private void Awake()
    {
        fullSize = transform.localScale;
    }
    private void OnEnable()
    {
        
        StartCoroutine(scaleUp());
    }

    public AnimationCurve scaleUpCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float scaleUpTime = 1f;
    private IEnumerator scaleUp() {
        float currTime = 0f;
        float lerpVal;
        WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        while (currTime < scaleUpTime) {
            currTime += Time.deltaTime;
            lerpVal = scaleUpCurve.Evaluate(Mathf.InverseLerp(0f, scaleUpTime, currTime));
            transform.localScale = lerpVal * fullSize;
            yield return wfeof;
        }
    }
}
