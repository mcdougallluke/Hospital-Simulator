using UnityEngine;
using System.Collections;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab; // Assign your NPC prefab in the inspector
    public Transform spawnPoint; // Assign a spawn point in the inspector
    public Transform pointA; // Assign in inspector
    public Transform pointB; // Assign in inspector

    private void Start()
    {
        StartCoroutine(SpawnNPCs());
    }

    private IEnumerator SpawnNPCs()
    {
        while (true) // This will keep spawning NPCs indefinitely
        {
            SpawnNPC();
            yield return new WaitForSeconds(5f); // Wait for 5 seconds before spawning the next NPC
        }
    }

    private void SpawnNPC()
    {
        GameObject npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
        NPCMovement npcMovement = npc.GetComponent<NPCMovement>();
        if (npcMovement != null)
        {
            // Set the points A and B for the spawned NPC
            npcMovement.pointA = pointA;
            npcMovement.pointB = pointB;
        }
    }
}
