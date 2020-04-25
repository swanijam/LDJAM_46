using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLock : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale <= .1f) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }
        if (Input.GetMouseButtonDown(0)) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetMouseButtonDown(2)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
