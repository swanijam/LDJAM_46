using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DiveController : MonoBehaviour
{
    // public vars
    public LookAtPlanet looker;
    public float dvelocity;
    public Transform camera;
    public float fullDescentTime = 10f;
    public float maxDistance = 20f, minDistance = 7f;
    public AnimationCurve camEffectCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float fovMin, fovMax;
    public float offsetTrackingRatio = .2f;
    public Vector2 shakeAmplitudeRange;
    public Vector2 shakeFreqRange;
    public CinemachineVirtualCamera diveCam;
    public CinemachineVirtualCamera planetEaterCam;
    public CinemachineVirtualCamera farCam;
    public Animator bunnyAnimator;
    public float ejectDrag = 1f;
    public GameObject lance;
    public AudioSource ambientSpace;
    public AudioSource ambientOrbit;
    public AudioSource DiveAir;
    
    // state vars
    Vector3 planetiSize = Vector3.one;
    Vector3 planetiOffset;
    Planet curPlanet;
    Transform curPlanetTarget;
    Rigidbody rb;
    float descentVelocity = 0f;
    float curDescentTime = 0f;
    bool fullCharge = false;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void InitializeWithPlanet(Planet p) {
        curPlanetTarget = p.transform;
        curPlanet = p;
        planetiSize = p.transform.localScale;
        planetiOffset = p.transform.position-transform.position;
        diveCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = shakeAmplitudeRange.x;
        diveCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = shakeFreqRange.x;

        // reset all values
        fullCharge = false;
        descentVelocity = 0f;
        curDescentTime = 0f;

    }

    void Update()
    {
        if (!fullCharge && Input.GetMouseButton(0)) {
            if (!DiveAir.isPlaying) DiveAir.Play();
            rb.velocity += camera.TransformDirection(GetInputTranslationDirection()) * accelPerSec;
            rb.velocity = rb.velocity.normalized * Mathf.Min(maxSpeed, rb.velocity.magnitude);
            descentVelocity = dvelocity;
            fullDescentTime += Time.deltaTime;
            scaleAfterDescent(descentVelocity);
        }

    }

    public void scaleAfterDescent (float velocity) {
        Debug.Log("scaling");
        float iradius = curPlanetTarget.localScale.x/2f;
        Vector3 planetToPlayer = (transform.position-curPlanetTarget.position).normalized;
        Vector3 surfacePosition = curPlanetTarget.position + planetToPlayer*iradius;
        float newScale_Scale = 1f + velocity * Time.deltaTime;
        float newScale_Offset = 1f + velocity * Time.deltaTime * offsetTrackingRatio;
        curPlanetTarget.localScale *= newScale_Scale;
        curPlanetTarget.position = surfacePosition + -planetToPlayer * iradius * (newScale_Offset);
        // Debug.Log(Vector3.Distance(surfacePosition, transform.position));
        float lerpVal = camEffectCurve.Evaluate(Mathf.InverseLerp(maxDistance, minDistance, Vector3.Distance(surfacePosition, transform.position)));
        // DiveAir.volume = lerpVal;
        // Debug.Log(lerpVal +", "+ Vector3.Distance(surfacePosition, transform.position));
        diveCam.m_Lens.FieldOfView = Mathf.Lerp(fovMax, fovMin, lerpVal);
        diveCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = Mathf.Lerp(shakeAmplitudeRange.x, shakeAmplitudeRange.y, lerpVal);
        diveCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = Mathf.Lerp(shakeFreqRange.x, shakeFreqRange.y, lerpVal);
        if (lerpVal >= .96f)  {
            fullCharge = true;
            StartCoroutine(FinisherSequence());
        }
    }
    [Header("FinisherSequence")]
    public float blackTime = .8f;
    public UnityEngine.Events.UnityEvent activateBlack;
    public float drillTime = 5f;
    public UnityEngine.Events.UnityEvent activateDrill;
    public float whiteTime = 1f;
    public UnityEngine.Events.UnityEvent activateWhite;
    public float exitTime = 4f;
    public UnityEngine.Events.UnityEvent activateExit;
    public UnityEngine.Events.UnityEvent afterExit;
    IEnumerator FinisherSequence() {
        // cover screen with black
        looker.enabled = false;
        activateBlack.Invoke();
        DiveAir.Stop();
        ambientOrbit.GetComponent<FadeVolume>().SetToMin();
        ambientSpace.Pause();
        yield return new WaitForSeconds(blackTime);

        lance.gameObject.SetActive(false);    
        bunnyAnimator.SetBool("lance", false);
        
        // drill time
        activateDrill.Invoke();
        yield return new WaitForSeconds(drillTime);

        farCam.transform.position = curPlanet.transform.position + curPlanet.transform.right * 50f + curPlanet.transform.up * 15f;
        // white time
        activateWhite.Invoke();
        yield return new WaitForSeconds(whiteTime);
        // hard cut to distant camera,
        // ambientOrbit.GetComponent<FadeVolume>().SetToMin();
        ambientSpace.Play();
        activateExit.Invoke();
        curPlanet.Explode();
        curPlanet.SpawnHeart();
        curPlanetTarget.transform.localScale = planetiSize;
        transform.position = curPlanetTarget.transform.position;
        transform.rotation = Quaternion.LookRotation(-Vector3.right, Vector3.up);
        looker.transform.localRotation = Quaternion.identity;
        rb.velocity = transform.forward * 25f;
        rb.drag = ejectDrag;
        // burst to particles
        // send to planet eater
        // planet eater happy emote
        yield return new WaitForSeconds(exitTime);
        // return camera to player
        curPlanet.HeartVulnerable();
        planetEaterCam.m_LookAt = curPlanet.Heart.transform;
        afterExit.Invoke();
        rb.drag = 5f;
    }

    public float accelPerSec = 5f;
    public float maxSpeed = 5f;
    public float forwardScale = 10f;
    public float verticalScale = .5f;
    Vector3 GetInputTranslationDirection()
    {
        Vector3 direction = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.up * verticalScale;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += -Vector3.up * verticalScale;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += -Vector3.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        // if (fullCharge && Input.GetMouseButton(0)) {
        //     direction += Vector3.forward * forwardScale;
        // }
        return direction.normalized;
    }
}
