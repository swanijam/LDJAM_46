using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatSequence : MonoBehaviour
{
    public bool runOnEnable = true;
    private void OnEnable()
    {
        if(runOnEnable) StartCoroutine(SpeakPhrases());
    }
    public TMP_Text text1, text2;
    public float charsPerSecond = 12f;
    public float prePause = 3f;
    public Phrase[] phrases;
    public Animator talkingAnimator;
    public Transform talkerLookat;
    public AudioClip[] syllables;
    public AudioSource speakerAudio;
    public int factor = 1; int count = 0;
    IEnumerator SpeakPhrases() {
        count = 0;
        Quaternion iRot = talkingAnimator.transform.rotation;
        if (talkerLookat != null) {
            yield return new WaitForSeconds(2f);
            Debug.Log("looking");
            float curTime = 0f;
            AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
            while(curTime < 1f) {
                curTime += Time.deltaTime;
                talkingAnimator.transform.rotation = Quaternion.Lerp(iRot, Quaternion.LookRotation(talkerLookat.position-talkingAnimator.transform.position, Vector3.up), curTime);
                yield return null;
            }
        }
        yield return new WaitForSeconds(prePause);
        for (int i = 0; i < phrases.Length; i++) {
            text1.maxVisibleCharacters = 0;
            text2.maxVisibleCharacters = 0;
            
            yield return new WaitForSeconds(phrases[i].delayTime);
            text1.text = phrases[i].text;
            text2.text = phrases[i].text;
            text1.ForceMeshUpdate();
            text2.ForceMeshUpdate();
            int charlength = text1.GetTextInfo(phrases[i].text).characterCount;
            Debug.Log(charlength +", "+ phrases[i].text);
            if (talkingAnimator != null) talkingAnimator.SetBool("Talking", true);
            for (int j = 1; j <= charlength; j++) {
                count = (count + 1) % factor;
                if (count == 0 && speakerAudio != null) {
                    speakerAudio.PlayOneShot(syllables[Random.Range(0, syllables.Length)]);
                }
                yield return new WaitForSeconds(1f/charsPerSecond);
                text1.maxVisibleCharacters = j;
                text2.maxVisibleCharacters = j;
            } 
            if (talkingAnimator != null) talkingAnimator.SetBool("Talking", false);
            yield return new WaitForSeconds(phrases[i].sustainTime);
            text1.text = "";
            text2.text = "";
        }
        if (talkerLookat != null) {
            Debug.Log("looking");
            float curTime = 0f;
            AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
            while(curTime < 1f) {
                curTime += Time.deltaTime;
                talkingAnimator.transform.rotation = Quaternion.Lerp(iRot, Quaternion.LookRotation(talkerLookat.position-talkingAnimator.transform.position, Vector3.up), 1f-curTime);
                yield return null;
            }
        }
        if (AfterChatSequence != null) AfterChatSequence.Invoke();
        yield return new WaitForSeconds(afterEventDelay);
        if (AfterChatSequenceDelayed != null) AfterChatSequenceDelayed.Invoke();
    }

    public UnityEngine.Events.UnityEvent AfterChatSequence;
    public float afterEventDelay = 4.7f;
    public UnityEngine.Events.UnityEvent AfterChatSequenceDelayed;
}

[System.Serializable]
public class Phrase {
    public float delayTime;
    [TextArea()]
    public string text;
    public float sustainTime;
}
