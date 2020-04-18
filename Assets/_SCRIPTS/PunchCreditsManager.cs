using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.TMPro;

public class PunchCreditsManager : MonoBehaviour
{  
    public Transform[] credits;
    public bool beginOnStart = true;
    public Vector3 maxSize = Vector3.one;
    // Start is called before the first frame update
    void Start()
    {
        if (beginOnStart) Begin();
    }
    public void Begin() {
        StartCoroutine(CreditsSlideShow());
    }

    public AnimationCurve _curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float _time = 1f;
    public float stayTime = 2f;
    public float fadeoutTime = 0f;
    public UnityEngine.Events.UnityEvent afterSlideShow;
    public IEnumerator CreditsSlideShow() {
        for (int i = 0; i < credits.Length; i++) {
            float currTime = 0f;
            float lerpVal;
            WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
            WaitForSeconds wfst = new WaitForSeconds(stayTime);
            credits[i].gameObject.SetActive(true);
            while (currTime < _time) {
                currTime += Time.deltaTime;
                lerpVal = _curve.Evaluate(Mathf.InverseLerp(0f, _time, currTime));
                credits[i].localScale = lerpVal * Vector3.one;
                yield return wfeof;
            }
            if (i+1 <= credits.Length) {
                yield return wfst;
            }
        }

        yield return new WaitForSeconds(fadeoutTime);
        afterSlideShow.Invoke();
    }
}
