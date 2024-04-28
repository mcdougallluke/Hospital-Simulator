using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void FootstepSound()
    {
        // Check if the GameObject has the "Player" tag
        if (gameObject.layer == LayerMask.NameToLayer("Player"))

        {
            audioManager.PlayMovementSound(audioManager.walking);
           // Debug.Log("Footstep sound played!");
        }
    }
    public void RunSound()
    {
        // Check if the GameObject has the "Player" tag
        if (gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            audioManager.PlayMovementSound(audioManager.running);
            Debug.Log("RUNNING sound played!");
        }
    }
}