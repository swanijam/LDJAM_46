using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImageInOut : MonoBehaviour
{
    public Color Max = Color.white, Min = Color.clear;
    
        public Vector2 varRange = Vector2.up; 
        public float time = 1f;
        public AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public Image _image;
        public Image image {
            get {
                if (_image == null) {
                    _image = GetComponent<Image>();
                }
                return _image;
            }
            set {
                _image = value;
            }
        }
        public void SetToMax() {
            image.color = Max;
        }
        public void SetToMin() {
            image.color = Min;
        }
        public void FadeToMax () {
            StartCoroutine(_FadeToMax(image));
        }
        public void FadeTextToMax (Image _r) {
            StartCoroutine(_FadeToMax(_r));
        }
    
        private IEnumerator _FadeToMax(Image r) {
            float currTime = 0f;
            Color icolor = r.color;
            while (currTime < time) {
                currTime += Time.deltaTime;
                float lerpVal = curve.Evaluate(Mathf.InverseLerp(0f, time, currTime));
            // Debug.Log("Fading to Max");
                //r.material.SetFloat(propertyName, Mathf.Lerp(varRange.x, varRange.y, lerpVal));
                r.color = Color.LerpUnclamped(icolor, Max, lerpVal);
                yield return new WaitForEndOfFrame();
            }
        }
    
        public void FadeToMin () {
            StartCoroutine(_FadeToMin(image));
        }
        public void FadeTextToMin (Image _r) {
            StartCoroutine(_FadeToMin(_r));
        }
        private IEnumerator _FadeToMin(Image r) {
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
