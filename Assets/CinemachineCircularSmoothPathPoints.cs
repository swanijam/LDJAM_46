using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[ExecuteInEditMode]
public class CinemachineCircularSmoothPathPoints : MonoBehaviour
{
    public CinemachineSmoothPath smoothPath;
    public float radius = 30f;
    public int numPoints = 32;
    public bool calculateNow = false;
    // Update is called once per frame
    void Update()
    {
       if (calculateNow) {
           calculateNow = false;
            List<CinemachineSmoothPath.Waypoint> points = new List<CinemachineSmoothPath.Waypoint>();
            Vector3 center = transform.position;
            Vector3 point = Vector3.forward * radius;
            CinemachineSmoothPath.Waypoint newPoint = new CinemachineSmoothPath.Waypoint();
            newPoint.position = point;
            newPoint.roll = 5f;
            points.Add(newPoint);
            for (int i = 0; i < numPoints; i++) {
                point = Quaternion.AngleAxis(360f / numPoints, Vector3.up) * point;
                newPoint = new CinemachineSmoothPath.Waypoint();
                newPoint.position = point;
                newPoint.roll = 5f;
                points.Add(newPoint);
            }
            smoothPath.m_Waypoints = points.ToArray();
        }
    }
}
