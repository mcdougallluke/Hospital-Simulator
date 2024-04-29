using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    CameraManager cameraManager;
    private bool freezeCamera = false;

    
    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (!freezeCamera) {
            cameraManager.HandleAllCameraMovement();
        }
    }

    public void SetCameraFreeze(bool freeze)
    {
        freezeCamera = freeze;
        Debug.Log("Camera freeze set to: " + freeze);
    }
}
