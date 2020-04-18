using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.TMPro;

public class SplashScreenCreditsManager : MonoBehaviour
{  
    public RectTransform[] credits;
    public bool beginOnStart = true;
    public TMPro.TMP_Text headerText;
    // Start is called before the first frame update
    void Start()
    {
        if (beginOnStart) Begin();
    }
    public void Begin() {
        StartCoroutine(CreditsSlideShow());
    }

    public AnimationCurve _curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public AnimationCurve _fadecurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float _time = 1f;
    public float stayTime = 2f;
    public UnityEngine.Events.UnityEvent afterSlideShow;
    public IEnumerator CreditsSlideShow() {
        for (int i = 0; i <= credits.Length; i++) {
            float currTime = 0f;
            float lerpVal;
            WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
            WaitForSeconds wfst = new WaitForSeconds(stayTime);
            while (currTime < _time) {
                currTime += Time.deltaTime;
                lerpVal = _curve.Evaluate(Mathf.InverseLerp(0f, _time, currTime));
                // exiting credit
                if (i-1 >= 0) {
                    credits[i-1].anchorMin = new Vector2(-lerpVal, 0f);
                    credits[i-1].anchorMax = new Vector2(1f-lerpVal, 1f);
                    credits[i-1].ForceUpdateRectTransforms();
                }
                // entering credit
                if (i < credits.Length) {
                    credits[i].anchorMin = new Vector2(1f-lerpVal, 0f);
                    credits[i].anchorMax = new Vector2(2f-lerpVal, 1f);
                    credits[i].ForceUpdateRectTransforms();
                }
                yield return wfeof;
            }
            if (i+1 <= credits.Length) {
                yield return wfst;
            }
        }

        float zcurrTime = 0f;
        float zlerpVal;
        WaitForEndOfFrame zwfeof = new WaitForEndOfFrame();
        Color icolor = headerText.color;
        float fadeoutTime = .6f;
        while (zcurrTime < fadeoutTime) {
            zcurrTime += Time.deltaTime;
            zlerpVal = _fadecurve.Evaluate(Mathf.InverseLerp(0f, fadeoutTime, zcurrTime));
            headerText.color = icolor * new Color(1f, 1f, 1f, zlerpVal);
            yield return zwfeof;
        }
        yield return new WaitForSeconds(fadeoutTime);
        afterSlideShow.Invoke();
    }
}
