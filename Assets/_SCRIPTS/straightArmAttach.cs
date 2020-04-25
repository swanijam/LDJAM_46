using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class straightArmAttach : MonoBehaviour
{
    public Transform shoulder;
    //  Quaternion iShoulderRot;
     Quaternion iShoulderRelative;
    //  Vector3 iHandPos;
    public Transform iHandPos;
    public Transform hand;
    public Transform grabTarget;
     Vector3 iElbowPos;
    private void Awake()
    {
        iShoulderRelative = Quaternion.FromToRotation(hand.position-shoulder.position, shoulder.forward);
     }
    public bool grabbing = false;
    Vector3 currentTarget;
    // Update is called once per frame
    float grabSmoothVel = 0f;
    float lerpVal = 0f;
    void Update()
    {
        lerpVal = Mathf.SmoothDamp(lerpVal, grabbing ? 1f : 0f, ref grabSmoothVel, .13f, 6f);
        currentTarget = Vector3.Lerp(iHandPos.position, grabTarget.position, lerpVal);
        shoulder.rotation = Quaternion.LookRotation(currentTarget - shoulder.position) * iShoulderRelative;
        hand.position = currentTarget;
    }
}
