using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeRoamController : MonoBehaviour
{
    Rigidbody rb;
    public Transform camera;
    public LookAtPlanet looker;
    public Animator flightAnimator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        looker.enabled = false;
        ffx = 0;
        ffy = 0;
        ffxVelocity = 0f;
        ffyVelocity = 0f;
    }

    public float lookRotationCatchupPerSecond = .5f;
    public Vector2 lookCatchupSoftRange = new Vector2(10f, 60f);
    // Update is called once per frame
    void Update()
    {
        rb.velocity += camera.TransformDirection(GetInputTranslationDirection()) * accelPerSec;
        rb.velocity = rb.velocity.normalized * Mathf.Min(maxSpeed, rb.velocity.magnitude);
        float lookCatchupMod = Mathf.InverseLerp(lookCatchupSoftRange.x, lookCatchupSoftRange.y, Vector3.Angle( looker.transform.forward, camera.forward));
        looker.transform.rotation = Quaternion.Slerp( looker.transform.rotation, camera.rotation, lookRotationCatchupPerSecond * Time.deltaTime * lookCatchupMod);
        float thrust = 0f;
        if (Input.GetKey(KeyCode.W)) {
            thrust += 1f;
        }
        if (Input.GetKey(KeyCode.S)) {
            thrust -= 1f;
        }
        rb.velocity += thrust * forwardScale * looker.transform.forward;
        
        Vector3 XZvelocity = Vector3.ProjectOnPlane(camera.InverseTransformDirection(rb.velocity), camera.up);
        XZvelocity = XZvelocity.normalized * (Mathf.Min(XZvelocity.magnitude, 1f));
        ffx = Mathf.SmoothDamp(ffx, XZvelocity.x, ref ffxVelocity, ffxSmoothTime, ffxMaxSpeed);
        ffy = Mathf.SmoothDamp(ffy, XZvelocity.z, ref ffyVelocity, ffySmoothTime, ffyMaxSpeed);
        flightAnimator.SetFloat("flyX", ffx);
        flightAnimator.SetFloat("flyY", ffy);
    }
    // smoothdamping blend tree axes
    float ffx = 0f, ffxVelocity = 0f, ffxSmoothTime = .15f, ffxMaxSpeed = 2f;
    float ffxTarget = 1f;
    float ffy = 0f, ffyVelocity = 0f, ffySmoothTime = .15f, ffyMaxSpeed = 2f;
    float ffyTarget = 1f;

    public float accelPerSec = 5f;
    public float maxSpeed = 5f;
    public float forwardScale = 10f;
    public float verticalScale = .5f;
    Vector3 GetInputTranslationDirection()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            direction += -Vector3.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        direction = direction.normalized;
        return direction;
    }
}
