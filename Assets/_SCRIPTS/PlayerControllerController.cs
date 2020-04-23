using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerControllerController : MonoBehaviour
{
    public static PlayerControllerController _instance;
    public static PlayerControllerController instance {
        get {
           return _instance;
        }
        set {
           if (_instance != null) {
               Debug.LogError("second singleton playercontrollercontroller instance found. Deleting the oldest one.");
               GameObject.Destroy(_instance);
           }
           _instance = value;
        }
    }
    private void Awake()
    {
        instance = this;
    }
    public int playerMaxHealth = 5;
    public Planet d_iPlanet;
    public FightOrbitController orbitController;
    public CinemachineVirtualCamera orbitCam;
    public CinemachineVirtualCamera takeDamageCam;
    public float ejectVelocity = 20f;
    public float ejectAttack = 1f;
    public float ejectSustain = .5f;
    public float ejectDecay = 2f;
    public Collider freeRoamCollider;
    public LookAtPlanet looker;
    public DiveController diveController;
    public CinemachineVirtualCamera diveCamBridge;
    public CinemachineVirtualCamera diveCam;
    public FreeRoamController freeRoamController;
    public CinemachineFreeLook freeCam;
    public CinemachineVirtualCamera freeIntermediateCam;
    public CinemachineVirtualCamera farCam;
    public healthbar healthbar;
    public static bool invincible = false;
    public AudioSource orbitAmbient;
    private void OnEnable()
    {
        // d_iPlanet.onHealthZero += BeginDescent;
        // BeginOrbitCombat(d_iPlanet);
        healthbar.varRange.y = playerMaxHealth;
        healthbar.health = playerMaxHealth;
        // freeWeight = freeRoamController.flightAnimator.GetLayerWeight(1);6
    }

    float freeWeight = 0f, freeWeightVelocity = 0f, freeWeightSmoothTime = .3f, freeWeightMaxSpeed = 1f;
    float freeWeightTarget = 1f;
    float orbitWeight = 0f, orbitWeightVelocity = 0f, orbitWeightSmoothTime = .3f, orbitWeightMaxSpeed = 1f;
    float orbitWeightTarget = 1f;
    private void Update()
    {
        freeWeightTarget = freeRoamController.enabled ? 1f : 0f;
        freeWeight = Mathf.SmoothDamp(freeWeight, freeWeightTarget, ref freeWeightVelocity, freeWeightSmoothTime, freeWeightMaxSpeed);
        freeRoamController.flightAnimator.SetLayerWeight(1, freeWeight);

        orbitWeightTarget = orbitController.enabled ? 1f : 0f;
        orbitWeight = Mathf.SmoothDamp(orbitWeight, orbitWeightTarget, ref orbitWeightVelocity, orbitWeightSmoothTime, orbitWeightMaxSpeed);
        freeRoamController.flightAnimator.SetLayerWeight(2, orbitWeight);
    }

    public void BeginOrbitCombat(Planet p) {
        // Debug.Log("Beginning Orbit Combat");
        orbitAmbient.GetComponent<FadeVolume>().FadeToMax();
        if (orbitController.enabled == true) return; // for redundant planet collisions
        freeRoamCollider.enabled = false;
        freeRoamController.enabled = false;
        orbitController.enabled = true;
        looker.planet = p.transform;
        looker.slow = true;
        looker.enabled = true;
        // freeCam.m_LookAt = p.transform;

        orbitController.inputEnabled = false; // to be anabled after delay
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        diveController.enabled = false;
        StartCoroutine(EnablePlanetInput(p));
        orbitController.InitializeWithPlanet(p);
        diveController.InitializeWithPlanet(p);
        p.onHealthZero += BeginDescent;
        farCam.gameObject.SetActive(false);
        diveCam.gameObject.SetActive(false);
        diveCamBridge.gameObject.SetActive(false);
        freeCam.gameObject.SetActive(false);
    }

    public AnimationCurve lookToPlanetCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float lookToPlanetTime = 1f;
    public float beginOrbitInputDelay = 3f;
    IEnumerator EnablePlanetInput(Planet p) {
        // freeCam.m_RecenterToTargetHeading.RecenterNow();
        // yield return new WaitForSeconds(1f);
        p.curTargetIdleSpin = 0f;
        freeCam.gameObject.SetActive(false);
        
        orbitCam.gameObject.SetActive(true);
        Transform temp = new GameObject("Temp").transform;
        temp.position = freeCam.m_LookAt.position;
        orbitCam.m_LookAt = temp;
        float currTime = 0f;
        float lerpVal;
        Vector3 iPosition = freeCam.m_LookAt.position;
        WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        while (currTime < lookToPlanetTime) {
            currTime += Time.deltaTime;
            lerpVal = lookToPlanetCurve.Evaluate(Mathf.InverseLerp(0f, lookToPlanetTime, currTime));
            temp.position = Vector3.Lerp(iPosition, p.transform.position, lerpVal);
            yield return wfeof;
        }
        orbitCam.m_LookAt = p.transform;
        GameObject.Destroy(temp.gameObject);
        p.StartCombat();
        orbitController.inputEnabled = true;
        looker.slow = false;
        looker.enabled = true;
        freeRoamCollider.enabled = true;
    }

    public void BeginDescent() {
        StartCoroutine(_BeginDescent());
    }

    IEnumerator _BeginDescent() {
        diveCam.m_Lens.FieldOfView = diveController.fovMax;
        diveCam.m_LookAt = orbitController.curTargetPlanet;
        freeRoamCollider.enabled = false;
        farCam.gameObject.SetActive(false);
        orbitController.SPEAR.gameObject.GetComponent<ScaleFader>().FadeToMin();
        yield return new WaitForSeconds(1.3f);
        orbitController.enabled = false;
        freeRoamController.enabled = false;

        orbitController.bunnyAnimator.SetBool("lance", true);
        diveController.lance.gameObject.SetActive(true);
        
        
        orbitCam.gameObject.SetActive(false);
        freeCam.gameObject.SetActive(false);
        
        diveCam.gameObject.SetActive(false);
        diveCamBridge.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        
        diveController.enabled = true;
        diveCam.gameObject.SetActive(true);
        diveCamBridge.gameObject.SetActive(false);
    }

    public void BeginFreeRoam() {
        freeRoamCollider.enabled = true;

        orbitController.enabled = false;
        diveController.enabled = false;
        freeRoamController.enabled = true;
        
        farCam.gameObject.SetActive(false);
        orbitCam.gameObject.SetActive(false);
        diveCam.gameObject.SetActive(false);
        diveCamBridge.gameObject.SetActive(false);
        freeCam.gameObject.SetActive(true);
    }

    public void TakeDamage() {
        if (invincible) return;
        healthbar.health--;
        orbitController.bunnyAnimator.SetTrigger("hurt");
        if (healthbar.health <= 0) {
            StartCoroutine(_TakeDamage());
        }
    }
    public void TakeAllDamage() {
        if (invincible) return;
        healthbar.health = 0;
        if (healthbar.health <= 0) {
            StartCoroutine(_TakeDamage());
        }
    }
    public IEnumerator _TakeDamage() {
        Debug.Log("DAMAGE TAKE");
        PlayerControllerController.invincible = true;
        orbitController.planet.StopAttacking();
        freeRoamCollider.enabled = false;
        orbitController.enabled = false;
        takeDamageCam.gameObject.SetActive(true);
        // ask orbit controller for planet to player vector
        Vector3 planetToPlayer = (transform.position-orbitController.curTargetPlanet.position).normalized;
        // add velocity away from planet
        GetComponent<Rigidbody>().velocity = planetToPlayer * ejectVelocity;
        float prevDrag = GetComponent<Rigidbody>().drag;// = planetToPlayer * ejectVelocity;
        GetComponent<Rigidbody>().drag = 1f;
        
        // activate free roam after delay 
        yield return new WaitForSeconds(ejectAttack);
        takeDamageCam.gameObject.SetActive(false);
        orbitCam.gameObject.SetActive(false);
        freeIntermediateCam.gameObject.SetActive(true);

        yield return new WaitForSeconds(ejectSustain); // wait for recentering through intermediateCam
        freeIntermediateCam.gameObject.SetActive(false);
        freeCam.m_BindingMode = CinemachineTransposer.BindingMode.LockToTargetWithWorldUp;
        freeCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(ejectDecay); // wait for recentering through intermediateCam
        freeCam.m_BindingMode = CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;

        freeRoamCollider.enabled = true;
        freeRoamController.enabled = true;
        GetComponent<Rigidbody>().drag = prevDrag;
        yield return new WaitForSeconds(.5f);
        healthbar.FadeToMax(); // return to max health
        PlayerControllerController.invincible = false;
    }
}
