using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class NPCHeadLookAt : MonoBehaviour
{
    [SerializeField] private Rig rig;
    [SerializeField] private Transform headLookAtTransform;

    private bool isLookingAtPosition = false;
    private float targetWeight = 0f;
    private float lerpSpeed = 2f;
    private const float threshold = 0.01f; // Threshold for considering the weight "close enough"

    private void Update()
    {
        // Adjust the target weight based on whether the NPC is supposed to be looking at something
        targetWeight = isLookingAtPosition ? 1f : 0f;
        
        // Use Mathf.Lerp to smoothly transition the rig's weight
        rig.weight = Mathf.Lerp(rig.weight, targetWeight, lerpSpeed * Time.deltaTime);

        // Check if the rig's weight is close enough to the target weight
        if (Mathf.Abs(rig.weight - targetWeight) < threshold)
        {
            // If the target weight was 1 (looking at position) and it's reached, stop looking
            if (isLookingAtPosition && targetWeight == 1f)
            {
                isLookingAtPosition = false; // Reset the flag once the position is reached
            }
        }
    }

    public void LookAtPosition(Vector3 position)
    {
        isLookingAtPosition = true;
        headLookAtTransform.position = position;
    }
}
