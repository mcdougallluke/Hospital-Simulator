using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {


    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            float interactRange = 1.5f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            
            foreach (Collider collider in colliderArray) {
                if (collider.TryGetComponent(out NPCInteractable interactable)) {
                    interactable.Interact(transform);
                }
            
            }
        }
    }

    public NPCInteractable GetInteractableObject() {
        List<NPCInteractable> interactableList = new List<NPCInteractable>();
        float interactRange = 1.5f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray) {
            if (collider.TryGetComponent(out NPCInteractable interactable)) {
                interactableList.Add(interactable);
            }
        }

        NPCInteractable closestInteractable = null;
        foreach (NPCInteractable interactable in interactableList) {
            if (closestInteractable == null) {
                closestInteractable = interactable;
            } else {
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) < 
                    Vector3.Distance(transform.position, closestInteractable.GetTransform().position)) {
                    // Closer
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;
    }

}