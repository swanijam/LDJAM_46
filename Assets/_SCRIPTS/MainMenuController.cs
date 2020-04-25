using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    public Canvas uiCanvas;
    public Button startButton;
    public Button controlsButton;
    public Button quitButton;
    public GameObject UIGroup;

    [Header("Planet Eater Animation")]
    public Animator planetEaterAnimator;
    public AnimationCurve planetEaterLerpAnimCurve;
    public float planetEaterLerpAnimLength;
    public Transform startTransform;
    public Transform endTransform;
    public Transform planetEaterTransform;
    public float eatAnimStartTime;

    [Header("Cameras")]
    public GameObject startCamera;
    public GameObject gameCamera;

    [Header("Other Elements")]
    public CursorLock cursorLock;

    public static bool gameStarted = false;
    private bool heEatedAlready = false;

    private void Awake() 
    {
        UIGroup.SetActive(true);
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void StartGame()
    {
        cursorLock.enabled = true;
        startButton.gameObject.SetActive(false);
        controlsButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        StartCoroutine(StartGameSequence());
        // Start opening sequence here
    }

    public IEnumerator StartGameSequence()
    {
        float animTime = 0;
        while(animTime <= 1)
        {
            animTime += Time.deltaTime/planetEaterLerpAnimLength;
            float lerpVal = planetEaterLerpAnimCurve.Evaluate(animTime);
            planetEaterTransform.position = Vector3.Lerp(startTransform.position, endTransform.position, lerpVal);
            yield return null;
            if(lerpVal >= eatAnimStartTime && !heEatedAlready)
            {
                Debug.Log("Play Eat Anim");
                planetEaterAnimator.SetTrigger("EatTitle");
                heEatedAlready = true;
            }
        }
        gameCamera.SetActive(true);
        startCamera.SetActive(false);
        uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        gameStarted = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
