using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFader : MonoBehaviour
{
    public Vector3 minScale, maxScale = Vector3.one;
            // Vector3 scale {
            //     get {
            //         return _scale;
            //     }
            //     set {
            //         _scale = value;
            //     }
            // }
        
        public float time = 1f;
        public AnimationCurve curvein = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public AnimationCurve curveout = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    
        public void FadeToMax () {
            StartCoroutine(_FadeToMax());
        }
        public void FadeVector3ToMax () {
            StartCoroutine(_FadeToMax());
        }
    
        private IEnumerator _FadeToMax() {
            float currTime = 0f;
            while (currTime < time) {
                currTime += Time.deltaTime;
                float lerpVal = curvein.Evaluate(Mathf.InverseLerp(0f, time, currTime));
                //r.material.SetFloat(propertyName, Mathf.Lerp(varRange.x, varRange.y, lerpVal));
                transform.localScale = Vector3.Lerp(maxScale, minScale, lerpVal);
                yield return new WaitForEndOfFrame();
            }
        }
    
        public void FadeToMin () {
            StartCoroutine(_FadeToMin());
        }
        public void FadeVector3ToMin () {
            StartCoroutine(_FadeToMin());
        }
        private IEnumerator _FadeToMin() {
            float currTime = 0f;
            while (currTime < time) {
                currTime += Time.deltaTime;
                float lerpVal = curveout.Evaluate(Mathf.InverseLerp(0f, time, currTime));
                //r.material.SetFloat(propertyName, Mathf.Lerp(varRange.y, varRange.x, lerpVal));
                transform.localScale = Vector3.Lerp(maxScale, minScale, lerpVal);
                yield return new WaitForEndOfFrame();
            }
        }
}
