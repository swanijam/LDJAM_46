using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public float launchSpeed = 3f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToReadyPosition () {
        transform.localPosition -= Vector3.forward * 2f;
    }

    public void Launch() {
        GameObject newgo = Instantiate(gameObject, transform.position, transform.rotation, null);
        newgo.AddComponent<BoxCollider>();
        Rigidbody rigidbody = newgo.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = false;
        rigidbody.velocity = launchSpeed * transform.forward;
        GetComponent<MeshRenderer>().enabled = false;
    }

    public float readyDelay = 3f;
    IEnumerator SpearReady() {
        yield return new WaitForSeconds(readyDelay);
        GetComponent<MeshRenderer>().enabled = true;
    }
}
