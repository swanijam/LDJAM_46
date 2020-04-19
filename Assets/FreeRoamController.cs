using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeRoamController : MonoBehaviour
{
    Rigidbody rb;
    public Transform camera;
    public LookAtPlanet looker;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        looker.enabled = false;
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
            // looker.transform.rotation = Quaternion.LookRotation()


            
    }

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
