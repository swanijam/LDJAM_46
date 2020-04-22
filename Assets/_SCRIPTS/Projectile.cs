using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Planet parentPlanet;
    public LayerMask layerMask;
    private void OnTriggerEnter(Collider other)
    {
        if (layerMask == (layerMask | (1 << other.gameObject.layer))) {
            Debug.Log("damaging??");
            other.gameObject.GetComponent<PlayerControllerController>().TakeDamage();
            // parentPlanet.StopAttacking();
        }
    }
}
