using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class straightArmAttach : MonoBehaviour
{
    public Transform shoulder;
    public Transform hand;
    public Transform grabTarget;
    public Transform elbow;
    
    // Update is called once per frame
    void Update()
    {
        shoulder.LookAt(grabTarget);
        hand.position = grabTarget.position;
        elbow.position = Vector3.Lerp(shoulder.position, hand.position, .5f);
    }
}
