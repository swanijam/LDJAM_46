using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerControllerController : MonoBehaviour
{
    public Planet d_iPlanet;
    public FightOrbitController orbitController;
    public CinemachineVirtualCamera orbitCam;
    public DiveController diveController;
    public CinemachineVirtualCamera diveCamBridge;
    public CinemachineVirtualCamera diveCam;
    public FreeRoamController freeRoamController;
    public CinemachineFreeLook freeCam;
    public CinemachineVirtualCamera farCam;
    private void OnEnable()
    {
        d_iPlanet.onHealthZero += BeginDescent;
        BeginOrbitCombat(d_iPlanet);
    }
    public void BeginOrbitCombat(Planet p) {
        freeRoamController.enabled = false;
        orbitController.enabled = true;
        diveController.enabled = false;
        orbitController.InitializeWithPlanet(p);
        diveController.InitializeWithPlanet(p);
        d_iPlanet.onHealthZero += BeginDescent;
        farCam.gameObject.SetActive(false);
        orbitCam.gameObject.SetActive(true);
        diveCam.gameObject.SetActive(false);
        diveCamBridge.gameObject.SetActive(false);
        freeCam.gameObject.SetActive(false);
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
