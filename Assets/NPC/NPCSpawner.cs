using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab; // Assign your NPC prefab in the Inspector
    public Transform[] spawnPoints; // Assign spawn points in the Inspector
    public Transform target; // The target that NPCs should move towards. Assign this in the Inspector.
    private int maxNPCs = 1; // Maximum number of NPCs to spawn
    private int currentNPCCount = 0; // Tracks the current number of spawned NPCs

    private void Start()
    {
        SpawnNPCs(); // Initial spawn when the game starts
    }

    void SpawnNPCs()
    {
        // Only spawn up to the maxNPCs limit
        while (currentNPCCount < maxNPCs)
        {
            SpawnNPC();
            currentNPCCount++;
        }
    }

    void SpawnNPC()
    {
        // Choose a random spawn point from the available spawn points
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnPointIndex];

        // Instantiate the NPC at the chosen spawn point
        GameObject npcInstance = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);

        // Assign the target to the NPC's movement script
        NPCMovement npcMovementScript = npcInstance.GetComponent<NPCMovement>();
        if (npcMovementScript != null)
        {
            npcMovementScript.target = target; // Set the target for the NPC
        }
    }

    // Call this method when an NPC is destroyed or despawns to keep count accurate
    public void NPCDestroyed()
    {
        currentNPCCount--;
        // Optional: Spawn a new NPC immediately after one is destroyed
        if (currentNPCCount < maxNPCs)
        {
            SpawnNPC();
            currentNPCCount++;
        }
    }
}
