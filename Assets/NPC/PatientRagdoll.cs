using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PatientRagdoll : MonoBehaviour
{       
    private NavMeshAgent agent; // Add this line
    public float forceThreshold = 5f; // Minimum force to trigger ragdoll
    private Animator animator;
    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColliders;
    public bool isDown = false; // Added isDown boolean

    void Start()
    {
        animator = GetComponent<Animator>();
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        agent = GetComponent<NavMeshAgent>(); // Initialize the NavMeshAgent reference

        SetRagdoll(false); // Start with ragdoll disabled
    }

    void SetRagdoll(bool state)
    {
        isDown = state;
        if (animator != null) animator.enabled = !state;

        foreach (var body in ragdollBodies)
        {
            body.isKinematic = !state;
        }
        foreach (var col in ragdollColliders)
        {
            col.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;
        GetComponent<Rigidbody>().isKinematic = state;

        if (agent != null) agent.enabled = !state; // Disable or enable the NavMeshAgent based on the ragdoll state
    }

    IEnumerator ResetRagdoll()
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds before getting up
        SetRagdoll(false);
        animator.Play("Getting Up");

        // Reset the NPC's navigation and state
        if (agent != null)
        {
            agent.enabled = true; // Re-enable the NavMeshAgent
            agent.ResetPath(); // Clear any existing path
        }

        PatientAI npcWandering = GetComponent<PatientAI>();
        if (npcWandering != null)
        {
            npcWandering.ResetAfterRagdoll(); // Add this method to your NPCWandering script
        }

        // Wait until the "Getting Up" animation is done playing
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Getting Up") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > forceThreshold && !isDown)
        {
            SetRagdoll(true);
            StartCoroutine(ResetRagdoll());
        }
    }
}
