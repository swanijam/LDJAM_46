using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlanet : MonoBehaviour
{
    public Transform planet;
    public bool slow = false;
    public float slowCatchUpPerSec = 1.6f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // transform.LookAt(planet, Vector3.up);
        Quaternion lookRot = Quaternion.LookRotation(planet.position-transform.position, Vector3.up);
        transform.rotation = slow ? Quaternion.Slerp(transform.rotation, lookRot, slowCatchUpPerSec*Time.deltaTime) : lookRot;
    }
}
