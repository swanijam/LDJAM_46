using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerControllerController : MonoBehaviour
{
    public Planet d_iPlanet;
    public FightOrbitController orbitController;
    public CinemachineVirtualCamera orbitCam;
    public Collider freeRoamCollider;
    public LookAtPlanet looker;
    public DiveController diveController;
    public CinemachineVirtualCamera diveCamBridge;
    public CinemachineVirtualCamera diveCam;
    public FreeRoamController freeRoamController;
    public CinemachineFreeLook freeCam;
    public CinemachineVirtualCamera farCam;
    private void OnEnable()
    {
        // d_iPlanet.onHealthZero += BeginDescent;
        // BeginOrbitCombat(d_iPlanet);
    }
    public void BeginOrbitCombat(Planet p) {
        freeRoamCollider.enabled = false;
        freeRoamController.enabled = false;
        orbitController.enabled = true;
        looker.enabled = true;
        looker.slow = true;
        orbitController.inputEnabled = false; // to be anabled after delay
        diveController.enabled = false;
        StartCoroutine(EnablePlanetInput());
        orbitController.InitializeWithPlanet(p);
        diveController.InitializeWithPlanet(p);
        p.onHealthZero += BeginDescent;
        farCam.gameObject.SetActive(false);
        orbitCam.gameObject.SetActive(true);
        diveCam.gameObject.SetActive(false);
        diveCamBridge.gameObject.SetActive(false);
        freeCam.gameObject.SetActive(false);
    }
    public float beginOrbitInputDelay = 3f;
    IEnumerator EnablePlanetInput() {
        yield return new WaitForSeconds(beginOrbitInputDelay);
        orbitController.inputEnabled = true;
        looker.slow = false;
    }

    public void BeginDescent() {
        StartCoroutine(_BeginDescent());
    }

    IEnumerator _BeginDescent() {
        farCam.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.3f);
        orbitController.enabled = false;
        freeRoamController.enabled = false;

        
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
}
