using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SatelliteModeController : MonoBehaviour
{
    public Planet m_Planet;
    Rigidbody rb;
    public CinemachineVirtualCamera camera;
    public float range = 30f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, m_Planet.transform.position) < range) camera.gameObject.SetActive(true);
        rb.velocity += camera.transform.TransformDirection(GetInputTranslationDirection());
        rb.velocity = Vector3.ProjectOnPlane(rb.velocity, (transform.position-m_Planet.transform.position));
    }

    public float accelPerSec = 5f;
    public float maxSpeed = 5f;
    public float verticalScale = .5f;
    Vector3 GetInputTranslationDirection()
    {
        Vector3 direction = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward * verticalScale;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += -Vector3.forward * verticalScale;
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
