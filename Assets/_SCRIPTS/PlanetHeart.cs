using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlanetHeart : MonoBehaviour
{
    public CinemachineVirtualCamera feedingCam;
    public Planet planetParent;
    public Transform planetEaterTarget;
    public Animator planetEaterAnimator;
    public AudioSource gulpAudio;
    private float CloseMouthDelayAmount = 0.8f;
    public void FeedToPlanetEater()
    {
        transform.SetParent(null);
        GetComponent<Collider>().enabled = false;
        feedingCam.m_LookAt = transform;
        StartCoroutine(feedToPE());
    }

    public AnimationCurve feedToPECurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float feedToPETime = 1f;
    private IEnumerator feedToPE()
    {

        planetEaterAnimator.SetBool("OpenMouth", true);
        Transform peater = planetEaterAnimator.transform;
        Quaternion irot = peater.rotation;
        feedingCam.gameObject.SetActive(true);
        float currTime = 0f;
        float lerpVal;
        Vector3 iPos = transform.position;
        WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        // StartCoroutine(CloseMouthDelay());
        bool mouthAnimationPlayed = false;
        while (currTime < feedToPETime)
        {
            currTime += Time.deltaTime;
            lerpVal = feedToPECurve.Evaluate(Mathf.InverseLerp(0f, feedToPETime, currTime));
            transform.position = Vector3.LerpUnclamped(iPos, planetEaterTarget.position, lerpVal * .989f);
            peater.rotation = Quaternion.Lerp(irot, Quaternion.LookRotation((transform.position - peater.position).normalized, Vector3.up), lerpVal);
            if(lerpVal > 0.5f && mouthAnimationPlayed == false){
                planetEaterAnimator.SetBool("OpenMouth", false);
                mouthAnimationPlayed = true;
                gulpAudio.PlayDelayed(2f);
            }
            yield return wfeof;
        }
        planetEaterAnimator.SetBool("OpenMouth", false);
        yield return new WaitForSeconds(2f);


        feedingCam.gameObject.SetActive(false);
        GameObject.DestroyImmediate(gameObject);
    }

    private IEnumerator CloseMouthDelay()
    {
        Debug.Log("We here");
        yield return new WaitForSeconds(CloseMouthDelayAmount);
        planetEaterAnimator.SetBool("OpenMouth", false);
    }

}
