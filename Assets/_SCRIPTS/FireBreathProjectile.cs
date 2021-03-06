﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreathProjectile : MonoBehaviour
{
    public Planet parentPlanet;
    public LayerMask layerMask;
    private void OnTriggerEnter(Collider other)
    {
        if (layerMask == (layerMask | (1 << other.gameObject.layer))) {
            other.gameObject.GetComponent<PlayerControllerController>().TakeAllDamage();
            // parentPlanet.StopAttacking();
        }
    }
}
