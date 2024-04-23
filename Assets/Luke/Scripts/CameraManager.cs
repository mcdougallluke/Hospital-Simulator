using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform targetTransform; // Target player transform
    public Transform cameraPivot;
    public Transform cameraTransform;
    public LayerMask collisionLayers;
    private float defaultPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;

    public float cameraCollisionRadius = 0.2f;
    public float minimumCollisionOffset = 0.2f;
    public float cameraCollisionOffset = 0.2f;
    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed = 2;
    public float cameraPivotSpeed = 2;

    public float lookAngle; // Horizontal rotation angle
    public float pivotAngle; // Vertical pivot angle
    public float minPivotAngle = -35;
    public float maxPivotAngle = 35;

    public Vector3 cameraOffset;
    
    private void Awake() {
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement() {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }

    private void FollowTarget() {
        // Calculate the offset position from the target based on the target's rotation
        Vector3 offsetPosition = targetTransform.TransformPoint(cameraOffset);

        // Smoothly interpolate the camera's position towards the offset position
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, offsetPosition, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }


    private void RotateCamera() {
        Vector3 rotation;
        Quaternion targetRotation;

        // Replace inputManager.cameraInputX/Y with Input.GetAxis calls
        lookAngle += (Input.GetAxis("Mouse X") * cameraLookSpeed);
        pivotAngle -= (Input.GetAxis("Mouse Y") * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        // Rotate the player object to match the camera's horizontal rotation
        if (targetTransform != null) {
            Vector3 playerRotation = Vector3.zero;
            playerRotation.y = lookAngle;
            targetTransform.rotation = Quaternion.Euler(playerRotation);
        }

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }


    private void HandleCameraCollisions() {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers)) {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition = -(distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffset) {
            targetPosition -= minimumCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
