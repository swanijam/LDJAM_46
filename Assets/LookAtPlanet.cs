using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlanet : MonoBehaviour
{
    public Transform planet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(planet, Vector3.up);
    }
}
