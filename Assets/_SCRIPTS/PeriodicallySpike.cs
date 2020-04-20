using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicallySpike : PeriodicallyAttack
{
    public float StickOutDuration = 6;
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(LaunchLoop());
    }
    public override IEnumerator LaunchLoop() {
        yield return new WaitForSeconds(Random.Range(0f, 8f));
        while(true) {
            if (!firing) {
                yield return new WaitForSeconds(interval);
                continue;
            }
            animator.SetBool("StickOut", true);
            yield return new WaitForSeconds(StickOutDuration);
            animator.SetBool("StickOut", false);
            yield return new WaitForSeconds(interval);
        }
    }
    public float projectileLifeTime = 6f;
    public override IEnumerator KillProjectile(GameObject g) {
        yield return new WaitForSeconds(projectileLifeTime);
        GameObject.Destroy(g);
    }
}
