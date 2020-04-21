using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackThruster : MonoBehaviour
{
    public Transform reference, plane;
    public Vector3 targetDirection = Vector3.forward;
    public float dotRange;
    public ParticleSystem jet;
    public float minMagnitude = .5f;
    // Start is called before the first frame update
    Vector3 ilookVector;
    void Start()
    {
        ilookVector = reference.InverseTransformDirection(transform.forward);
        prevPosition = transform.position;
    }
    Vector3 prevPosition;
    // Update is called once per frame
    void Update()
    {
        Vector3 delta = (transform.position-prevPosition);
        Vector3 lookVector = reference.TransformDirection(ilookVector);
        Debug.Log(delta.magnitude);
        if (delta.magnitude > minMagnitude && Mathf.Abs(1f-Vector3.Dot(targetDirection, reference.TransformDirection(-delta.normalized))) < dotRange) {
            lookVector = Vector3.ProjectOnPlane(-delta, plane.forward);
            // transform.rotation = Quaternion.LookRotation(lookVector, transform.up);
            jet.enableEmission = true;
        } else {
            jet.enableEmission = false;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookVector, transform.up), .95f * Time.deltaTime);
        prevPosition = transform.position;
    }
}
