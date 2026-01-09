using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public float lookSpeedX = 2f; // test
    public float lookSpeedY = 2f;
    public Transform playerBody;

    private float xRotation = 0f;
    private bool isCursorLocked = true;

    void Start()
    {
        // Start with the cursor locked
        SetCursorLock(true);
    }

    void Update()
    {
        if (isCursorLocked)
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeedX;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeedY;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
    }
    
        public void SetCursorLock(bool lockCursor)
        {
            isCursorLocked = lockCursor;
            Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !lockCursor;
        }
    
        public void UnlockCursor()
        {
            isCursorLocked = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }   
}
