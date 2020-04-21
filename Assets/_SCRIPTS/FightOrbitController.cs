using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FightOrbitController : MonoBehaviour
{
    public Transform camera;
    public CinemachineVirtualCamera orbitCam;
    public Transform curTargetPlanet;
    public Planet planet;
    public float orbitRadius = 20f;
    public float orbitSoftWidth = 5f;
    public float orbitCorrectForce = 1f;
    public float dodgeForce = 20f;
    public float dodgeCooldown = 1.5f;
    public LookAtPlanet looker;
    public Spear SPEAR;
    public Animator bunnyAnimator;
    public AimLine aim;
    Rigidbody rb;
    [HideInInspector]
    public bool inputEnabled;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void InitializeWithPlanet(Planet _planet) {
        curTargetPlanet = _planet.transform;
        planet = _planet;
        SPEAR.Ready();
    }

    public float accelPerSec = 5f;
    public float maxSpeed = 5f;
    public float verticalScale = .5f;
    bool spearReady = false;
    // Update is called once per frame
    float dashCooldown = 0f;
    bool dashReady;
    bool dashing;
    public float dashTime = 1f;
    float _dashTime = 0f;
    float spearTime = 0f;
    void Update()
    {
        if (!dashing) {
            Vector3 inputDirection = GetInputTranslationDirection();
            if (inputEnabled) rb.velocity += camera.TransformDirection(inputDirection) * accelPerSec;
            rb.velocity = rb.velocity.normalized * Mathf.Min(maxSpeed, rb.velocity.magnitude);
            // Debug.Log(Vector3.Distance(transform.position, curTargetPlanet.position));
            // begin windup
            if (SPEAR.spearIsReady && Input.GetMouseButtonDown(0)) {
                spearReady = true;
                spearTime = 0f;
                if (bunnyAnimator != null) bunnyAnimator.SetBool("throwWindUp", true);
            }
            // continue windup
            if (spearReady && Input.GetMouseButton(0)) {
                spearTime += Time.deltaTime;
                float lerpVal = Mathf.InverseLerp(0f, .617f, spearTime);
                if (aim != null) aim.strength = lerpVal;
                 orbitCam.m_Lens.FieldOfView = Mathf.Lerp(FOVRange.x, FOVRange.x-15f, lerpVal);
            }
            // release windup
            if (spearReady && !Input.GetMouseButton(0)) {
                spearReady = false;
                 if (aim != null) aim.strength = 0f;
                if (SPEAR.Launch() ) {
                    if (bunnyAnimator != null) bunnyAnimator.SetBool("throwWindUp", false);
                    StartCoroutine(animateFOV());
                }
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                StartCoroutine(Dash(inputDirection * dodgeForce));
            }
        }
        // Debug.Log(Vector3.Distance(transform.position, curTargetPlanet.position));
        if (Vector3.Distance(transform.position, curTargetPlanet.position) > orbitRadius + orbitSoftWidth) {
            rb.velocity += orbitCorrectForce * (curTargetPlanet.position - transform.position) * Time.deltaTime;
        } else if (Vector3.Distance(transform.position, curTargetPlanet.position) < orbitRadius - orbitSoftWidth) {
            rb.velocity += -orbitCorrectForce * (curTargetPlanet.position - transform.position) * Time.deltaTime;
        }
        // if (!dashReady) {
        //     dashCooldown += Time.deltaTime;
        //     if (dashCooldown > dodgeCooldown) dashReady = true;
        // }
        //     Debug.Log("DASHING");
        //     dashReady = false;
        //     dashCooldown = 0f;
        //     dashing = true;
        //     _dashTime = 0f;
        //     rb.velocity = inputDirection * dodgeForce;
        // }
        // } else {
        //     _dashTime += Time.deltaTime;
        //     if (_dashTime > dashTime) dashing = false;
        // }
    }

        public AnimationCurve animateFOVCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public float animateFOVTime = .25f;
        public Vector2 FOVRange;
        private IEnumerator animateFOV() {
            float currTime = 0f;
            float lerpVal;
            float iFOV = orbitCam.m_Lens.FieldOfView;
            WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
            while (currTime < animateFOVTime) {
                currTime += Time.deltaTime;
                lerpVal = animateFOVCurve.Evaluate(Mathf.InverseLerp(0f, animateFOVTime, currTime));
                if (lerpVal > .95f) iFOV = FOVRange.x;
                orbitCam.m_Lens.FieldOfView = Mathf.Lerp(iFOV, FOVRange.y, lerpVal);
                yield return wfeof;
            }
        }

    	public AnimationCurve DashCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public float DashTime = 1f;
        private IEnumerator Dash(Vector3 iVelocity) {
            float currTime = 0f;
            PlayerControllerController.invincible = true;
            dashing = true;
            float lerpVal;
            WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
            while (currTime < DashTime) {
                currTime += Time.deltaTime;
                lerpVal = DashCurve.Evaluate(Mathf.InverseLerp(0f, DashTime, currTime));
                // rb.velocity = camera.TransformDirection(Vector3.Lerp(iVelocity, Vector3.zero, lerpVal));
                rb.velocity = Vector3.ProjectOnPlane(camera.TransformDirection(Vector3.Lerp(iVelocity, Vector3.zero, lerpVal)), (transform.position-curTargetPlanet.position).normalized);
                yield return wfeof;
            }
            PlayerControllerController.invincible = false;
            dashing = false;
        }

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
        return direction.normalized;
    }
}
