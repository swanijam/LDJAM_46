using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlsMenuController : MonoBehaviour
{
    private CanvasGroupAlphaFader _canvasAlpha;
    public CanvasGroupAlphaFader canvasAlpha
    {
        get { 
            if (_canvasAlpha == null) _canvasAlpha = GetComponent<CanvasGroupAlphaFader>();
            return _canvasAlpha; 
        }
        set { _canvasAlpha = value; }
    }
    
    public Button okayButton, reloadButton;

    // Start is called before the first frame update
    void Start()
    {
        okayButton.onClick.AddListener(Okay);
        reloadButton.onClick.AddListener(Reload);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (canvasAlpha.canvasGroup.alpha > .5f) {
                Time.timeScale = 1f;
                Debug.Log("Timescale set up to " + Time.timeScale);
                GetComponent<UnityEngine.UI.GraphicRaycaster>().enabled = false;
                canvasAlpha.FadeToMin();
            } else {
                Time.timeScale = 0f;
                Debug.Log("Timescale set down to " + Time.timeScale);
                GetComponent<UnityEngine.UI.GraphicRaycaster>().enabled = true;
                canvasAlpha.FadeToMax();
            }
        }
    }

    public void Okay() {
        Time.timeScale = 1f;
        Debug.Log("Timescale set up to " + Time.timeScale);
        GetComponent<UnityEngine.UI.GraphicRaycaster>().enabled = false;
        canvasAlpha.FadeToMin();
    }

    public void Reload() {
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
}
