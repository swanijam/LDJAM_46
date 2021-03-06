﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public float launchSpeed = 3f;
    Vector3 fullSpearScale; 
    public GameObject trail;
    bool startZero = true;
    public AudioSource resummonSound;
    // public straightArmAttach grabby;
    // Start is called before the first frame update
    void Start()
    {
        fullSpearScale = transform.localScale;
        if (startZero) transform.localScale = Vector3.zero;
    }

    public void Ready() {
        // Debug.Log("ready??");
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
        yield return new WaitForSeconds(.4f);
        // grabby.grabbing = false;
        yield return new WaitForSeconds(readyDelay-.4f);
        float currTime = 0f;
        float lerpVal;
        WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        bool played = false;
        while (currTime < ReadySpearAnimTime) {
            currTime += Time.deltaTime;
            lerpVal = ReadySpearAnimCurve.Evaluate(Mathf.InverseLerp(0f, ReadySpearAnimTime, currTime));
            if (lerpVal > 1f && !played) {
                resummonSound.Play();
                played = true;
            }
            transform.localScale = fullSpearScale * lerpVal;
            yield return wfeof;
        }
        spearIsReady = true;
    }

    public LayerMask planetSurfLayer;
    public LayerMask planetWeakPointLayer;
    public LayerMask heartLayer;

    public GameObject impactVFXPrefab, weakPointVFXPrefab;
    public AudioSource hitMiss, hitWeakPoint;
    public AudioClip hitImpact1, hitImpact2;
    public bool hitsurf = false, hitweakPoint = false;
    Collision cacheCollisionSurf;
    Collision cacheCollisionWeakPt;
    Vector3 cacheCollisionNormal;
    private void OnCollisionEnter(Collision other)
    {
        Vector3 normal = -transform.forward;
        if (planetSurfLayer == (planetSurfLayer | (1 << other.gameObject.layer))) {
            hitsurf = true;
            cacheCollisionSurf = other;
            cacheCollisionNormal = normal;
        }
        if (planetWeakPointLayer == (planetWeakPointLayer | (1 << other.gameObject.layer))) {
            hitweakPoint = true;
            other.gameObject.GetComponent<WeakPoint>().TakeHit();
            cacheCollisionWeakPt = other;
            cacheCollisionNormal = normal;
        }
        if (heartLayer == (heartLayer | (1 << other.gameObject.layer))) {
            GameObject vfx = Instantiate(weakPointVFXPrefab, other.GetContact(0).point + normal, Quaternion.LookRotation(normal));
            vfx.transform.up = normal;
            other.gameObject.GetComponent<PlanetHeart>().FeedToPlanetEater();
            other.gameObject.GetComponent<PlanetHeart>().planetParent.KillThePlanet();
            PlayerControllerController.instance.BeginFreeRoam();
            hitWeakPoint.PlayOneShot(hitWeakPoint.clip);
            hitMiss.PlayOneShot(hitMiss.clip);
        }
    }

    void FixedUpdate()
    {
        Respond2Collision();
        // Debug.Log(cacheCollision == null);
        cacheCollisionNormal = transform.forward;
        cacheCollisionSurf = null;
        cacheCollisionWeakPt = null;
        hitsurf = false;
        hitweakPoint = false;
    }

    private void Respond2Collision() {
        
        if (hitweakPoint) {
            Vector3 normal = cacheCollisionNormal;
            GameObject vfx = Instantiate(weakPointVFXPrefab, cacheCollisionWeakPt.GetContact(0).point + normal, Quaternion.LookRotation(normal));
            vfx.transform.up = normal;
            // vfx.transform.up = normal;
            // stick into weak points
            // Debug.Log("HIT WEAK POINT", cacheCollisionWeakPt.gameObject);
            transform.position = cacheCollisionWeakPt.GetContact(0).point + cacheCollisionWeakPt.GetContact(0).normal * 1f;
            transform.rotation = Quaternion.LookRotation((cacheCollisionWeakPt.GetContact(0).point-transform.position).normalized);
            GetComponent<Collider>().enabled = false;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            transform.SetParent(cacheCollisionWeakPt.transform);
            hitWeakPoint.PlayOneShot(hitImpact1);
            hitWeakPoint.PlayOneShot(hitImpact2);
            hitWeakPoint.PlayDelayed(1f);
        }
        else if (hitsurf) {
        Vector3 normal = cacheCollisionNormal;
            // bounce off planet surf
            GameObject vfx = Instantiate(impactVFXPrefab, cacheCollisionSurf.GetContact(0).point + normal, Quaternion.LookRotation(normal));
            vfx.transform.up = normal;
            hitMiss.PlayOneShot(hitMiss.clip);
        }
        if (hitweakPoint || hitsurf) GetComponent<Collider>().enabled = false;
    }
}
