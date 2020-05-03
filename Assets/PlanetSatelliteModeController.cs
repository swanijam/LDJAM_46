using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSatelliteModeController : MonoBehaviour
{
    public Planet m_Planet;
    // Start is called before the first frame update
    void Start()
    {
        m_Planet.onEnterPlanet += BeginSatellite;
    }

    public void BeginSatellite() {
        Debug.Log("Player is in satellite mode (?)");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
