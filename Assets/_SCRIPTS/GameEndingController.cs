using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndingController : MonoBehaviour
{
    public Planet[] planets;
    bool endingTriggered = false;
    private void Update()
    {
        bool allplanetsDead = true;
        for(int i = 0; i < planets.Length; i++) {
            if (planets[i] != null) {
                allplanetsDead = false;
            } else {
                allplanetsDead = allplanetsDead && true;
            }
        }
        if (allplanetsDead && !endingTriggered) {
            StartCoroutine(EndingRoutine());
        }
    }

    public FadeImageInOut fade2black;
    public Transform player;
    public Transform teleportTo;
    public GameObject dialogueCam;
    public Transform flyto;
    public Animator planetEaterAnimator;
    // dialog object is on the camera.
    // public GameObject dialogObject;

    IEnumerator EndingRoutine() {
        endingTriggered = true;
        yield return new WaitForSeconds(4f);
        fade2black.FadeToMax();
        yield return new WaitForSeconds(4f);
        player.GetComponent<PlayerControllerController>().ShutDown();
        // player.GetComponent<PlayerControllerController>().looker.planet = null;
        player.position = teleportTo.position;
        player.rotation = teleportTo.rotation;
        dialogueCam.SetActive(true);
        fade2black.FadeToMin();
        // dialogObject.SetActive(true);
    }

    public void EatPlayerNow() {
        StartCoroutine(eatPlayer());
    }
    public AnimationCurve eatPlayerCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float eatPlayerTime = 2f;
    private IEnumerator eatPlayer() {
        float currTime = 0f;
        float lerpVal;
        Vector3 iPos = player.position;
        WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        planetEaterAnimator.SetBool("FinalNomOpen", true);
        while (currTime < eatPlayerTime) {
            currTime += Time.deltaTime;
            lerpVal = eatPlayerCurve.Evaluate(Mathf.InverseLerp(0f, eatPlayerTime, currTime));
            player.position = Vector3.Lerp(iPos, flyto.position, lerpVal*.989f);
            yield return wfeof;
        }
        planetEaterAnimator.SetBool("FinalNomOpen", false);
        yield return new WaitForSeconds(4f);
        fade2black.FadeToMax();
        yield return new WaitForSeconds(6f);
        // save any game data here
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
}
