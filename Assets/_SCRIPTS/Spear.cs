using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public float launchSpeed = 3f;
    Vector3 fullSpearScale; 
    public GameObject trail;
    bool startZero = true;
    // Start is called before the first frame update
    void Start()
    {
        fullSpearScale = transform.localScale;
        if (startZero) transform.localScale = Vector3.zero;
    }

    public void Ready() {
        Debug.Log("ready??");
        StartCoroutine(ReadySpearAnim());
    }

    public void MoveToReadyPosition () {
        transform.localPosition -= Vector3.forward * 2f;
    }
    public bool spearIsReady = false;
    public bool Launch() {
        if (!spearIsReady) return false;
        GameObject newgo = Instantiate(gameObject, transform.position, transform.rotation, null);
        newgo.GetComponent<Spear>().startZero = false;
        newgo.GetComponent<Spear>().spearIsReady = false;
        newgo.transform.localScale = fullSpearScale;
        newgo.GetComponent<BoxCollider>().enabled = true;
        Rigidbody rigidbody = newgo.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = false;
        rigidbody.velocity = launchSpeed * transform.forward;
        GameObject newTrail = Instantiate(trail, trail.transform.position, trail.transform.rotation, newgo.transform);
        newTrail.SetActive(true);
    //  /   GetComponent<MeshRenderer>().enabled = false;
        transform.localScale = Vector3.zero;
        spearIsReady = false;
        StartCoroutine(ReadySpearAnim());
        return true;
    }



    public float readyDelay = 3f;
    public AnimationCurve ReadySpearAnimCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float ReadySpearAnimTime = 1f;
    private IEnumerator ReadySpearAnim() {
        yield return new WaitForSeconds(readyDelay);
        float currTime = 0f;
        float lerpVal;
        WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        while (currTime < ReadySpearAnimTime) {
            currTime += Time.deltaTime;
            lerpVal = ReadySpearAnimCurve.Evaluate(Mathf.InverseLerp(0f, ReadySpearAnimTime, currTime));
            transform.localScale = fullSpearScale * lerpVal;
            yield return wfeof;
        }
        spearIsReady = true;
    }

    public LayerMask planetSurfLayer;
    public LayerMask planetWeakPointLayer;

    public GameObject impactVFXPrefab, weakPointVFXPrefab;
    private void OnCollisionEnter(Collision other)
    {
        Vector3 normal = -transform.forward;
        if (planetSurfLayer == (planetSurfLayer | (1 << other.gameObject.layer))) {
            // bounce off planet surf
            GameObject vfx = Instantiate(impactVFXPrefab, other.GetContact(0).point + normal, Quaternion.LookRotation(normal));
            vfx.transform.up = normal;
        }
        if (planetWeakPointLayer == (planetWeakPointLayer | (1 << other.gameObject.layer))) {
            GameObject vfx = Instantiate(weakPointVFXPrefab, other.GetContact(0).point + normal, Quaternion.LookRotation(normal));
            vfx.transform.up = normal;
            // vfx.transform.up = normal;
            // stick into weak points
            Debug.Log("HIT WEAK POINT");
            transform.position = other.GetContact(0).point + other.GetContact(0).normal * 1f;
            transform.rotation = Quaternion.LookRotation((other.GetContact(0).point-transform.position).normalized);
            GetComponent<Collider>().enabled = false;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            transform.SetParent(other.transform);
            other.gameObject.GetComponent<WeakPoint>().TakeHit();
        }
    }
}
