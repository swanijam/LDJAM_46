using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    public Button startButton;
    public Button controlsButton;
    public Button quitButton;
    public Button controlsScreenMenuButton;
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

    private bool heEatedAlready = false;

    private void Awake() 
    {
        UIGroup.SetActive(true);
        startButton.onClick.AddListener(StartGame);
        // controlsButton   // no listener for the controlsButton because Jim already did it with the inspector events
        quitButton.onClick.AddListener(QuitGame);
        controlsScreenMenuButton.onClick.AddListener(Reload);
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
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void Reload() {
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
}
