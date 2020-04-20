using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothDampToTarget : MonoBehaviour
{
    public Transform target;
    public float smoothTime = .5f;
    public float maxSpeed = 20f;

    Vector3 currentVelocity;
    Vector3 currentPosition;
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref currentVelocity, smoothTime, maxSpeed);
    }
}
