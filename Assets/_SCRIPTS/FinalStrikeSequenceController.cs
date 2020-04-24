using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FinalStrikeSequenceController : MonoBehaviour
{
    [Header("Cinemachine Settings")]
    public CinemachineVirtualCamera virtualCam;
    public float startAmplitude = 0.08f;
    public float startFrequency = 4.83f;
    public float fullTiltAmplitude = 0.18f;
    public float fullTiltFrequency = 12.6f;

    [Header("Heart Anim Settings")]
    public Transform heartObject;
    public float heartAnimLength;
    public AnimationCurve heartLerpAnimCurve;
    public AnimationCurve heartScaleAnimCurve;
    public Transform startTransform;
    public Transform endTransform;
    
    [Header("Timing Settings")]
    public float stage1Pause = 2;
    public float stage2Pause = 3;
    public float stage3Pause = 3;

    [Header("PlanetEater Settings")]
    public Animator planetEaterAnimator;

    public void OnEnable()
    {
        StartCoroutine(SequenceRoutine());
    }

    public IEnumerator SequenceRoutine()
    {
        heartObject.gameObject.SetActive(false);
        virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = startAmplitude;
        virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = startFrequency;

        yield return new WaitForSeconds(stage1Pause);

        virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = fullTiltFrequency;

        yield return new WaitForSeconds(stage2Pause);

        virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = fullTiltAmplitude + .3f;
        yield return new WaitForSeconds(1);
        virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = fullTiltAmplitude;

        yield return new WaitForSeconds(stage3Pause);


        heartObject.gameObject.SetActive(true);
        // heartObject.transform.position = startTransform.position;
        float animTime = 0;
        while(animTime <= 1)
        {
            animTime += Time.deltaTime/heartAnimLength;
            heartObject.position = Vector3.Lerp(startTransform.position, endTransform.position, heartLerpAnimCurve.Evaluate(animTime));
            heartObject.localScale = Vector3.Lerp(startTransform.localScale, endTransform.localScale, heartScaleAnimCurve.Evaluate(animTime));
            yield return null;
        }
        Debug.Log("ended the anim thing");
        planetEaterAnimator.SetBool("OpenMouth", false);
        ResetHeart();
    }

    public void ResetHeart()
    {
        Debug.Log("reset the heart");
        heartObject.position = startTransform.position;
        heartObject.localScale = startTransform.localScale;
        heartObject.gameObject.SetActive(false);
    }
}
