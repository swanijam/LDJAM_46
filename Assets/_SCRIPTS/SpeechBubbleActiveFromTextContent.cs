using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleActiveFromTextContent : MonoBehaviour
{
    public GameObject bubble;
    public TMPro.TMP_Text text;

    private void Update()
    {
        bubble.SetActive(!(text.text.Equals("")));
    }
}
