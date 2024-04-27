using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    CameraManager cameraManager;
    public bool freezeCamera = false;

    
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
}
