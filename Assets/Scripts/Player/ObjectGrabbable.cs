using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{

    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    private Collider collider;
    AudioManager audioManager;
    private void Awake() {
        objectRigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }
    public void Grab(Transform objectGrabPointTransform) {
        audioManager.PlaySFX(audioManager.pickupItem);
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
        collider.enabled = false;
    }

    public void Drop() {
        audioManager.PlaySFX(audioManager.pickupItem);
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;
        collider.enabled = true;
    }

    private void FixedUpdate() {
        if (objectGrabPointTransform != null) {
            // Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.fixedDeltaTime * 15f);
            // objectRigidbody.MovePosition(newPosition);
            objectRigidbody.MovePosition(objectGrabPointTransform.position);
        }
    }
}
