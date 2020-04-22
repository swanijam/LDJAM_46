using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    public LayerMask layerMask;
    public float strength = 0f;
    LineRenderer lineRenderer;
    public float maxAlpha = .4f;
    Color clearWhite = Color.white;
    private void OnEnable()
    {
        lineRenderer = GetComponent<LineRenderer>();
        clearWhite.a = 0f;
        lineRenderer.startColor = clearWhite;
    }
    private void LateUpdate()
    {
        lineRenderer.SetPosition(0, transform.position);
        Color newc = Color.white;
        newc.a = strength * maxAlpha;
        lineRenderer.endColor = newc;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 30f, layerMask)) {
            lineRenderer.SetPosition(1, hit.point);
        } else {
            lineRenderer.SetPosition(1, transform.position + transform.forward * 30f);
        }
    }
}
