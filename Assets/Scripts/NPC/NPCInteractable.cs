using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IIInteractable {

    [SerializeField] private string interactText;
    [SerializeField] private string responseText;
    public PatientAI patientAI;

    private Animator animator;
    private NPCHeadLookAt npcHeadLookAt;

    private void Awake() {
        animator = GetComponent<Animator>();
        npcHeadLookAt = GetComponent<NPCHeadLookAt>();
    }

    public void Interact(Transform interactorTransform) {
        Debug.Log("Interacting with NPC");

        // Calculate the direction to look at by subtracting the current position from the target's position.
        Vector3 directionToLookAt = interactorTransform.position - transform.position;
        // Make sure the rotation only affects the y-axis by zeroing out the x and z components.
        directionToLookAt.y = 0;

        // Create a rotation that looks along the calculated direction, keeping the up vector pointing upwards.
        Quaternion targetRotation = Quaternion.LookRotation(directionToLookAt);

        // Apply the rotation to the NPC instantly. For smoother rotation, you could use Quaternion.Slerp in the Update method.
        transform.rotation = targetRotation;

        npcHeadLookAt.LookAtPosition(interactorTransform.position + Vector3.up * 1.6f);
        ChatBubble3D.Create(transform.transform, new Vector3(-.3f, 1.7f, 0f), ChatBubble3D.IconType.Neutral, responseText);
        animator.SetTrigger("Talk");
        patientAI.StartSelectedMinigame();
    }

    public string GetInteractText() {
        return interactText;
    }

    public Transform GetTransform() {
        return transform;
    }

}