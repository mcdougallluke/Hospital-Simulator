using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IIInteractable {

    [SerializeField] private string interactText;
    // Removed the static responseText field
    public PatientAI patientAI;
    private bool isInteracting = false;

    private Animator animator;
    private NPCHeadLookAt npcHeadLookAt;

    private void Awake() {
        animator = GetComponent<Animator>();
        npcHeadLookAt = GetComponent<NPCHeadLookAt>();
    }

    public void Interact(Transform interactorTransform) {

        if (isInteracting) return;
        isInteracting = true;
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

        // Determine the response based on the minigame
        string responseText = DetermineResponseText();
        
        ChatBubble3D.Create(transform.transform, new Vector3(-.3f, 1.7f, 0f), ChatBubble3D.IconType.Neutral, responseText);
        animator.SetTrigger("Talk");
        StartCoroutine(ResetInteraction());

    }

    private IEnumerator ResetInteraction() {
    yield return new WaitForSeconds(1f); // Wait for 1 second or the time that fits your game flow
    isInteracting = false;
    }

    private string DetermineResponseText() {

        if (patientAI.currentState == PatientState.InExamRoom || patientAI.currentState == PatientState.PlayingMinigame) {
            // Existing conditions for minigame responses
            if (patientAI.selectedMinigameIndex == 0) { // Spelling Minigame

               List<string> responses = new List<string>() {
                "I sneezed so much my nose fell off",
                "My mom said I eat like a dog",
                "I think my stomach has left my body",
                "Is it normal to hear screams when I blink",
                "My knees glow in the dark",
                "I can talk to the sun",
                "My belly button is whispering secrets to me at night",
                "My throat feels like a cheese grater",
                "I'm sweating so much from my feet",
                "My brain is on fire!",
                "I've been hiccupping for a week straight"
            };

                
                // Randomly select a response
                int index = Random.Range(0, responses.Count);
                patientAI.StartSelectedMinigame();
                patientAI.currentState = PatientState.PlayingMinigame;
                return responses[index];
                
            }
            
            if (patientAI.selectedMinigameIndex == 1) { // Fetch Minigame

            List<string> responses = new List<string>() {
                "I'm sick I swear, bring me the ",
                "I'm feeling really sad, I need a ",
                "Sometimes I can't focus, I definitely need a ",
                "I'm feeling tired, I need the ",
                "I really need the ",
                "Big party this weekend, I need a ",
                "I'm so bored, I need a ",
                "My face burns, I need the ",
                "I'm here to pick up a perscription for the ",
                "I heard pills are really great, get me the ",
                "I'll slip you a 20 if you get me the "
            };

                patientAI.StartSelectedMinigame();
                patientAI.currentState = PatientState.PlayingMinigame;
                int index = Random.Range(0, responses.Count);
                return responses[index] + patientAI.desiredPill;
                //return "Bring me the " 
            }

            else if (patientAI.selectedMinigameIndex == 2) { // Arrow Input Minigame
            List<string> responses = new List<string>() {
                "I hope you're not as nervous as I am",
                "Is this gonna give me superpowers?",
                "Can we do this tomorrow? I forgot to set my DVR",
                "Make sure you put everything back where you found it",
                "Tell my family I love them... and to clear my browser history",
                "Any chance we can get this done in time for happy hour?",
                "Do my bones look weird to you?",
                "Can this get me out of jury duty?",
                "Is this gonna hurt? I'm not good with pain",
                "I checked WebMD and I think I just need a Band-Aid.",
                "Want to grab dinner after this?"
            };

            int index = Random.Range(0, responses.Count);

            patientAI.StartSelectedMinigame();
            patientAI.currentState = PatientState.PlayingMinigame;
            return responses[index];

            }
        }

        else{
            return "I'm not feeling well";
        }
        

        return ""; // Default response if none match
    }

    public string GetInteractText() {
        return interactText;
    }

    public Transform GetTransform() {
        return transform;
    }

}
