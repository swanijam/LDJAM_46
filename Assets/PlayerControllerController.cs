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
    public CinemachineVirtualCamera diveCam;
    public FreeRoamController freeRoamController;
    public CinemachineVirtualCamera freeCam;
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
        orbitCam.gameObject.SetActive(true);
        diveCam.gameObject.SetActive(false);
        freeCam.gameObject.SetActive(false);
    }

    public void BeginDescent() {
        orbitController.enabled = false;
        diveController.enabled = true;
        orbitCam.gameObject.SetActive(false);
        diveCam.gameObject.SetActive(true);
        freeCam.gameObject.SetActive(false);
    }
}
