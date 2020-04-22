using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicallyLaser : PeriodicallyAttack
{
    public float LaserDuration = 6;
    public LaserVFXController laserVFXController;
    public LayerMask layerMask;
    public GameObject eye;
    public float lookSpeed = 1;
    public GameObject player;
    private Coroutine lookAt;
    private Coroutine resetLook;
    private Quaternion startRot;
    public float resetLookDuration = 0.3f;
    public AnimationCurve resetLookCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float distanceThreshold = 13f;


    private void OnEnable()
    {
        startRot = eye.transform.localRotation;
        StartCoroutine(LaunchLoop());
    }
    public override IEnumerator LaunchLoop()
    {
        StartCoroutine(IsColliding(player));
        yield return new WaitForSeconds(Random.Range(0f, 8f));
        while (true)
        {
            if (!firing)
            {
                yield return new WaitForSeconds(interval);
                continue;
            }

            if (Random.Range(0f, 1f) < 0.8f && animator.GetBool("Open"))
            {
                laserVFXController.StartEyeLaserSequence();
                yield return new WaitForSeconds(laserVFXController.sphereChargeAnimLength + laserVFXController.beamAnimLength);
            }
            // else
            // {
            //     animator.SetTrigger("Blink");
            // }

            yield return new WaitForSeconds(interval);
        }
    }
    public float projectileLifeTime = 6f;
    public override IEnumerator KillProjectile(GameObject g)
    {
        yield return new WaitForSeconds(projectileLifeTime);
        GameObject.Destroy(g);
    }

    public IEnumerator IsColliding(GameObject player)
    {
        while (true)
        {
            if (Vector3.Distance(player.transform.position, eye.transform.position) < distanceThreshold && animator.GetBool("Laser") == false)
            {
                animator.SetBool("Open", true);
                if(lookAt == null) lookAt = StartCoroutine(LookAtPlayer(player.gameObject));
                if (resetLook != null) StopCoroutine(resetLook);
                resetLook = null;
            }
            else if (Vector3.Distance(player.transform.position, eye.transform.position) < distanceThreshold && animator.GetBool("Laser") == true)
            {
                if (lookAt != null) StopCoroutine(lookAt);
                lookAt = null;
                animator.SetBool("Open", true);
                yield return new WaitForSeconds(.4f);
                if(resetLook == null)resetLook = StartCoroutine(ResetLookRotation());
            }
            else
            {
                animator.SetBool("Open", false);
                if (lookAt != null) StopCoroutine(lookAt);
                lookAt = null;
                if (resetLook != null) StopCoroutine(resetLook);
                resetLook = null;
            }
            yield return null;
        }
    }

    public IEnumerator ResetLookRotation()
    {
        eye.transform.localRotation = startRot;
        // float animTime = 0;
        // Quaternion currentRot = transform.localRotation;
        // while (animTime <= 1)
        // {
        //     animTime += Time.deltaTime / resetLookDuration;
        //     eye.transform.localRotation = Quaternion.Slerp(currentRot, startRot, resetLookCurve.Evaluate(animTime));
        //     yield return null;
        // }
            yield return null;
    }

    public IEnumerator LookAtPlayer(GameObject player)
    {
        while (true)
        {
            Vector3 playerDirection = (player.transform.position - eye.transform.position).normalized;
            eye.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerDirection, -transform.right), 0.93f);
            yield return null;
        }
    }
}
