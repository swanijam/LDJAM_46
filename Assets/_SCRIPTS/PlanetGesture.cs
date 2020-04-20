using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGesture : MonoBehaviour
{
    public enum FaceState { ANGRY, HAPPY, SAD, HURT, INHALEFIRE, EXHALEFIRE, SMIRK, GRIMACE, IDLE };
    public FaceState facestate = FaceState.IDLE;
    public FaceState currentState = FaceState.IDLE;

    public SkinnedMeshRenderer planetFace;

    public float duration = 0.25f;
    public AnimationCurve animationCurve;

    void Update()
    {
        if (currentState != facestate)
        {
            SetFace(facestate);
            currentState = facestate;
        }
    }

    public void SetFace(FaceState state)
    {
        facestate = state;

        StopAllCoroutines();
        for (int i = 0; i < planetFace.sharedMesh.blendShapeCount; i++)
        {
            if ((int)facestate == i && i != (int)FaceState.IDLE)
            {
                StartCoroutine(FaceAnimation(i, 100));
            }
            else if (i != (int)FaceState.IDLE)
            {
                StartCoroutine(FaceAnimation(i, 0));
            }
        }
    }

    public IEnumerator FaceAnimation(int iState, float amount)
    {
        float journey = 0f;
        Debug.Log("Set Gesture");
        float start = planetFace.GetBlendShapeWeight((int)iState);
        while (journey <= duration)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / duration);

            float curvePercent = animationCurve.Evaluate(percent);
            planetFace.SetBlendShapeWeight((int)iState, Mathf.Lerp(start, amount, percent));
            yield return null;
        }
    }
}
