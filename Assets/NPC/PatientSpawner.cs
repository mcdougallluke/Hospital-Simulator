using UnityEngine;
using System.Collections;

public class PatientSpawner : MonoBehaviour
{
    public GameObject npcPrefab; // Assign in the inspector
    public Transform spawnPoint; // Assign a transform as the spawn point in the inspector
    public SpellingMinigame spellingMinigame; // Assign in the inspector
    public Score scoreScript; // Assign in the inspector
    public Transform initialPoint; // Assign in the inspector
    public Transform[] optionalPoints; // Assign in the inspector

    private float spawnCooldown = 2.0f;
    private int maxNPCCount = 3;

    void Start()
    {
        StartCoroutine(SpawnNPCRoutine());
    }

    private IEnumerator SpawnNPCRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnCooldown);
            SpawnNPCIfPossible();
        }
    }

    private void SpawnNPCIfPossible()
    {
        if (GameObject.FindObjectsOfType<PatientAI>().Length < maxNPCCount)
        {
            SpawnNPC();
        }
    }

    private void SpawnNPC()
    {
        // Instantiate the NPC at the spawn point
        GameObject npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);

        // Get the NPCWandering component and assign the references
        PatientAI npcWanderingScript = npc.GetComponent<PatientAI>();
        if (npcWanderingScript != null)
        {
            npcWanderingScript.spellingMinigame = spellingMinigame;
            npcWanderingScript.scoreScript = scoreScript;
            npcWanderingScript.initialPoint = initialPoint;
            npcWanderingScript.optionalPoints = optionalPoints;
        }
    }
}
