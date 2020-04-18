using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideOut : MonoBehaviour
{
    RectTransform myRT;
    // Start is called before the first frame update
    void Start()
    {
        myRT = GetComponent<RectTransform>();
    }
    bool deactivateAfterSlideOut = true;
    public void slideOutNow() {
        StartCoroutine(slide());
    }
	public AnimationCurve slideCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float slideTime = 1f;
    private IEnumerator slide() {
        float currTime = 0f;
        float lerpVal;
        WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        while (currTime < slideTime) {
            currTime += Time.deltaTime;
            lerpVal = slideCurve.Evaluate(Mathf.InverseLerp(0f, slideTime, currTime));
            myRT.anchorMin = new Vector2(0f-lerpVal, 0f);
            myRT.anchorMax = new Vector2(1f-lerpVal, 1f);
            yield return wfeof;
        }
    }
}
