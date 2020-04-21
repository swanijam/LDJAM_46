using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicallyLaser : PeriodicallyAttack
{
    public float LaserDuration = 6;
    public LaserVFXController laserVFXController;
    private void OnEnable()
    {
        StartCoroutine(LaunchLoop());
    }
    public override IEnumerator LaunchLoop() {
        yield return new WaitForSeconds(Random.Range(0f, 8f));
        while(true) {
            if (!firing) {
                yield return new WaitForSeconds(interval);
                continue;
            }
            // if(Random.Range(0,1) > 0.6f)
            // {
                laserVFXController.StartEyeLaserSequence();
            // } else {
            //     animator.SetTrigger("Blink");
            // }
            
            yield return new WaitForSeconds(interval);
        }
    }
    public float projectileLifeTime = 6f;
    public override IEnumerator KillProjectile(GameObject g) {
        yield return new WaitForSeconds(projectileLifeTime);
        GameObject.Destroy(g);
    }
}
