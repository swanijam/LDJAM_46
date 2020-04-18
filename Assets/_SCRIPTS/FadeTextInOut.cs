using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTextInOut : MonoBehaviour
{
    public Color Max = Color.white, Min = Color.clear;
    public TMPro.TMP_Text _text;
            TMPro.TMP_Text text {
                get {
                    if (_text == null) {
                        _text = GetComponent<TMPro.TMP_Text>();
                    }
                    return _text;
                }
                set {
                    _text = value;
                }
            }
        
        public Vector2 varRange = Vector2.up; 
        public float time = 1f;
        public AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    
        public void FadeToMax () {
            StartCoroutine(_FadeToMax(text));
        }
        public void FadeTextToMax (TMPro.TMP_Text _r) {
            StartCoroutine(_FadeToMax(_r));
        }
    
        private IEnumerator _FadeToMax(TMPro.TMP_Text r) {
            float currTime = 0f;
            Color icolor = r.color;
            while (currTime < time) {
                currTime += Time.deltaTime;
                float lerpVal = curve.Evaluate(Mathf.InverseLerp(0f, time, currTime));
                //r.material.SetFloat(propertyName, Mathf.Lerp(varRange.x, varRange.y, lerpVal));
                r.color = Color.LerpUnclamped(icolor, Max, lerpVal);
                yield return new WaitForEndOfFrame();
            }
        }
    
        public void FadeToMin () {
            StartCoroutine(_FadeToMin(text));
        }
        public void FadeTextToMin (TMPro.TMP_Text _r) {
            StartCoroutine(_FadeToMin(_r));
        }
        private IEnumerator _FadeToMin(TMPro.TMP_Text r) {
            float currTime = 0f;
            Color icolor = r.color;
            while (currTime < time) {
                currTime += Time.deltaTime;
                float lerpVal = curve.Evaluate(Mathf.InverseLerp(0f, time, currTime));
                //r.material.SetFloat(propertyName, Mathf.Lerp(varRange.y, varRange.x, lerpVal));
                r.color = Color.LerpUnclamped(icolor, Min, lerpVal);
                yield return new WaitForEndOfFrame();
            }
        }
}
