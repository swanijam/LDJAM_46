using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiveController : MonoBehaviour
{
    Planet curPlanet;
    Transform curPlanetTarget;
    Rigidbody rb;
    public Transform camera;
    // Start is called before the first frame update
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity += camera.TransformDirection(GetInputTranslationDirection()) * accelPerSec;
        rb.velocity = rb.velocity.normalized * Mathf.Min(maxSpeed, rb.velocity.magnitude);
    }

    public void InitializeWithPlanet(Planet p) {
        curPlanetTarget = p.transform;
    }

    public float accelPerSec = 5f;
    public float maxSpeed = 5f;
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

        if (Input.GetMouseButton(0)) {
            direction += Vector3.forward;
        }
        return direction.normalized;
    }
}
