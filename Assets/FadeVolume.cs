using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeVolume : MonoBehaviour
{
    public float maxVolume = 1f;
    public AudioSource _volume;
            AudioSource volume {
                get {
                    if (_volume == null) {
                        _volume = GetComponent<AudioSource>();
                    }
                    return _volume;
                }
                set {
                    _volume = value;
                }
            }
        
        public Vector2 varRange = Vector2.up; 
        public float time = 1f;
        public AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    
        public void FadeToMax () {
            StartCoroutine(_FadeToMax(volume));
        }
        public void FadeAudioSourceToMax (AudioSource _r) {
            StartCoroutine(_FadeToMax(_r));
        }
    
        private IEnumerator _FadeToMax(AudioSource r) {
            float currTime = 0f;
            while (currTime < time) {
                currTime += Time.deltaTime;
                float lerpVal = curve.Evaluate(Mathf.InverseLerp(0f, time, currTime));
                //r.material.SetFloat(propertyName, Mathf.Lerp(varRange.x, varRange.y, lerpVal));
                volume.volume = lerpVal * maxVolume;
                yield return new WaitForEndOfFrame();
            }
        }
    
        public void FadeToMin () {
            StartCoroutine(_FadeToMin(volume));
        }
        public void FadeAudioSourceToMin (AudioSource _r) {
            StartCoroutine(_FadeToMin(_r));
        }
        private IEnumerator _FadeToMin(AudioSource r) {
            float currTime = 0f;
            while (currTime < time) {
                currTime += Time.deltaTime;
                float lerpVal = curve.Evaluate(1f-Mathf.InverseLerp(0f, time, currTime));
                //r.material.SetFloat(propertyName, Mathf.Lerp(varRange.y, varRange.x, lerpVal));
                volume.volume = lerpVal * maxVolume;
                yield return new WaitForEndOfFrame();
            }
        }
}
