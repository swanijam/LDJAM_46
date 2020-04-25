using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroupAlphaFader : MonoBehaviour
{
    public CanvasGroup _canvasGroup;
            CanvasGroup canvasGroup {
                get {
                    if (_canvasGroup == null) {
                        _canvasGroup = GetComponent<CanvasGroup>();
                    }
                    return _canvasGroup;
                }
                set {
                    _canvasGroup = value;
                }
            }
        
        public float time = 1f;
        public AnimationCurve curve;
        public CursorLock cursorLock;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (canvasGroup.alpha > .5f) {
                    FadeToMin();
                    Time.timeScale = 1f;
                } else {
                    FadeToMax();
                    Time.timeScale = 0f;
                }
            }
        }
    
        public void FadeToMax () {
            StartCoroutine(_FadeToMax(canvasGroup));
        }
        public void FadeCanvasGroupToMax (CanvasGroup _r) {
            StartCoroutine(_FadeToMax(_r));
        }
    
        private IEnumerator _FadeToMax(CanvasGroup r) {
            float currTime = 0f;
            r.interactable = true;
            r.blocksRaycasts = true;
            cursorLock.enabled = false;
            while (currTime < time) {
                currTime += Time.unscaledDeltaTime;
                float lerpVal = curve.Evaluate(Mathf.InverseLerp(0f, time, currTime));
                canvasGroup.alpha = lerpVal;
                yield return new WaitForEndOfFrame();
            }
        }
    
        public void FadeToMin () {
            StartCoroutine(_FadeToMin(canvasGroup));
        }
        public void FadeCanvasGroupToMin (CanvasGroup _r) {
            StartCoroutine(_FadeToMin(_r));
        }
        private IEnumerator _FadeToMin(CanvasGroup r) {
            float currTime = 0f;
            r.interactable = false;
            r.blocksRaycasts = false;
            cursorLock.enabled = true;
            while (currTime < time) {
                currTime += Time.unscaledDeltaTime;
                float lerpVal = curve.Evaluate(Mathf.InverseLerp(0f, time, currTime));
                canvasGroup.alpha = 1f-lerpVal;
                yield return new WaitForEndOfFrame();
            }
        }
}
