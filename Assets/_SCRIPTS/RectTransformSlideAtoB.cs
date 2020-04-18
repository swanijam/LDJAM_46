using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectTransformSlideAtoB : MonoBehaviour
{
    RectTransform myRT;
    public Vector2 Amin = Vector2.zero;
    public Vector2 Amax = Vector2.one;
    public Vector2 Bmin = Vector2.zero;
    public Vector2 Bmax = Vector2.one;
    // Start is called before the first frame update
    void Start()
    {
        myRT = GetComponent<RectTransform>();
    }
    bool deactivateAfterSlideOut = true;
    public void SlideNow() {
        StartCoroutine(slide());
    }
	public AnimationCurve slideCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float slideTime = 1f;
    public UnityEngine.Events.UnityEvent afterSlide;
    private IEnumerator slide() {
        float currTime = 0f;
        float lerpVal;
        WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        while (currTime < slideTime) {
            currTime += Time.deltaTime;
            lerpVal = slideCurve.Evaluate(Mathf.InverseLerp(0f, slideTime, currTime));
            myRT.anchorMin = Vector2.LerpUnclamped(Amin, Bmin, lerpVal);
            myRT.anchorMax = Vector2.LerpUnclamped(Amax, Bmax, lerpVal);
            yield return wfeof;
        }
        afterSlide.Invoke();
    }

    public void SlideReverse() {
        StartCoroutine(slidereverse());
    }
    public AnimationCurve slideReverseCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float slideReverseTime = 1f;
    public UnityEngine.Events.UnityEvent afterSlideReverse;
    private IEnumerator slidereverse() {
        float currTime = 0f;
        float lerpVal;
        WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        while (currTime < slideReverseTime) {
            currTime += Time.deltaTime;
            lerpVal = slideReverseCurve.Evaluate(Mathf.InverseLerp(0f, slideReverseTime, currTime));
            myRT.anchorMin = Vector2.LerpUnclamped(Bmin, Amin, lerpVal);
            myRT.anchorMax = Vector2.LerpUnclamped(Bmax, Amax, lerpVal);
            yield return wfeof;
        }
        afterSlideReverse.Invoke();
    }
}

