using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private float pickUpRange = 5f;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private Transform objectGrabPointTransform;

    private ObjectGrabbable objectGrabbable;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (objectGrabbable == null) {
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit hit, pickUpRange, pickUpLayerMask)) {
                    if (hit.transform.TryGetComponent(out objectGrabbable)) {
                        objectGrabbable.Grab(objectGrabPointTransform);
                    }
            }
            } else {
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
            
        }
        
    }
}
