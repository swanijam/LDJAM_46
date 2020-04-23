using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeLightIntensity : MonoBehaviour
{
    	public AnimationCurve scaleSomethignUpCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public float scaleSomethignUpTime = 1f;
        private IEnumerator scaleSomethignUp() {
            float currTime = 0f;
            float lerpVal;
            WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
            while (currTime < scaleSomethignUpTime) {
                currTime += Time.deltaTime;
                lerpVal = scaleSomethignUpCurve.Evaluate(Mathf.InverseLerp(0f, scaleSomethignUpTime, currTime));
                transform.localScale = Vector3.one * lerpVal;
                yield return wfeof;
            }
        }
}
