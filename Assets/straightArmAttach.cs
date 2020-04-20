using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class straightArmAttach : MonoBehaviour
{
    public Transform shoulder;
    public Quaternion iShoulderRot;
    public Vector3 iHandPos;
    public Transform hand;
    public Transform grabTarget;
    public Transform elbow;
    public Vector3 iElbowPos;
    private void Awake()
    {
        iShoulderRot = shoulder.localRotation;
        iHandPos = hand.localPosition;
        iElbowPos = hand.localPosition;
    }
    // Update is called once per frame
    void Update()
    {
        shoulder.LookAt(grabTarget);
        hand.position = grabTarget.position;
        elbow.position = Vector3.Lerp(shoulder.position, hand.position, .5f);
    }
}
