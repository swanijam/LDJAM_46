using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class straightArmAttach : MonoBehaviour
{
    public Transform shoulder;
    public Quaternion iShoulderRot;
    public Quaternion iShoulderRelative;
    public Vector3 iHandPos;
    public Transform hand;
    public Transform grabTarget;
    public Transform elbow;
    public Vector3 iElbowPos;
    private void Awake()
    {
        iShoulderRot = shoulder.localRotation;
        iShoulderRelative = Quaternion.FromToRotation(hand.position-shoulder.position, shoulder.forward);
        iHandPos = hand.localPosition;
        iElbowPos = hand.localPosition;
    }
    public bool grabbing = false;
    // Update is called once per frame
    void Update()
    {
        if (grabbing) {
            shoulder.rotation = Quaternion.LookRotation(grabTarget.position - shoulder.position) * iShoulderRelative;
            hand.position = grabTarget.position;
            elbow.position = Vector3.Lerp(shoulder.position, hand.position, .5f);
        } else {
            shoulder.localRotation = iShoulderRot;
            hand.localPosition = iHandPos;
            elbow.localPosition = iElbowPos;
        }
    }
}
