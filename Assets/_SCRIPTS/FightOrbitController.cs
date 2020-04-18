using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightOrbitController : MonoBehaviour
{
    public Transform camera;
    public Transform curTargetPlanet;
    Planet planet;
    public float orbitRadius = 20f;
    public float orbitSoftWidth = 5f;
    public float orbitCorrectForce = 1f;
    public Spear SPEAR;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void InitializeWithPlanet(Planet _planet) {
        curTargetPlanet = _planet.transform;
        planet = _planet;
    }

    public float accelPerSec = 5f;
    public float maxSpeed = 5f;
    public float verticalScale = .5f;
    bool spearReady = false;
    // Update is called once per frame
    void Update()
    {
        rb.velocity += camera.TransformDirection(GetInputTranslationDirection()) * accelPerSec;
        rb.velocity = rb.velocity.normalized * Mathf.Min(maxSpeed, rb.velocity.magnitude);
        Debug.Log(Vector3.Distance(transform.position, curTargetPlanet.position));
        if (Vector3.Distance(transform.position, curTargetPlanet.position) > orbitRadius + orbitSoftWidth) {
            rb.velocity += orbitCorrectForce * (curTargetPlanet.position - transform.position) * Time.deltaTime;
        } else if (Vector3.Distance(transform.position, curTargetPlanet.position) < orbitRadius - orbitSoftWidth) {
            rb.velocity += -orbitCorrectForce * (curTargetPlanet.position - transform.position) * Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0)) {
         spearReady = true;
        }
        if (spearReady && Input.GetMouseButtonUp(0)) {
            spearReady = false;
            SPEAR.Launch();
        }
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
