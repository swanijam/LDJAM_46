using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicallyLaunchObject : MonoBehaviour
{
    public float interval = 20f;
    public GameObject projectile;
    public Vector3 localLaunchVector = Vector3.up;
    public float launchSpeed = 20f;
    public bool firing = false;
    public Planet parentPlanet;
    public Animator animator;
    public float instatiateUpVector;
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(LaunchLoop());
    }
    IEnumerator LaunchLoop() {
        yield return new WaitForSeconds(Random.Range(0f, 5f));
        while(true) {
            if (!firing) {
                yield return new WaitForSeconds(interval);
                continue;
            }
            animator.SetTrigger("Spit");
            yield return new WaitForSeconds(0.67f);
            GameObject go = Instantiate(projectile, transform.position+transform.up*instatiateUpVector, Quaternion.LookRotation(transform.TransformDirection(localLaunchVector), Vector3.up), null);
            go.GetComponent<Projectile>().parentPlanet = parentPlanet;
            go.transform.localScale = projectile.transform.localScale;
            go.SetActive(true);
            go.GetComponent<Rigidbody>().velocity = go.transform.forward * launchSpeed;
            StartCoroutine(KillProjectile(go));
            yield return new WaitForSeconds(interval);
        }
    }
    public float projectileLifeTime = 6f;
    IEnumerator KillProjectile(GameObject g) {
        yield return new WaitForSeconds(projectileLifeTime);
        GameObject.Destroy(g);
    }
}
