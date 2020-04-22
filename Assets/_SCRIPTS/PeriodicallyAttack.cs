using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicallyAttack : MonoBehaviour
{
    public float interval = 20f;
    public bool firing = false;
    public Planet parentPlanet;
    public Animator animator;
    
    public virtual IEnumerator LaunchLoop() {
        yield return new WaitForSeconds(Random.Range(0f, 5f));
        while(true) {
            if (!firing) {
                yield return new WaitForSeconds(interval);
                continue;
            }
            yield return new WaitForSeconds(interval);
        }
    }
    public virtual IEnumerator KillProjectile(GameObject g) {
        yield return null;
    }
}
