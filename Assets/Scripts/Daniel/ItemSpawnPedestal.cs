using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSpawnPedestal : MonoBehaviour
{
    public List<GameObject> itemPrefabs; // List of item prefabs to spawn
    public float spawnInterval = 5f; // Interval between spawns
    private float timer = 0f;
    public Transform spawnPoint; // Manually assign the spawn point in the Inspector

    void Start()
    {
        if (spawnPoint == null)
        {
            Debug.LogError("Spawn point not assigned! Assign the spawn point GameObject in the Inspector.");
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnItem();
            timer = 0f; // Reset timer
        }
    }
    void SpawnItem()
    {
        if (itemPrefabs.Count == 0)
        {
            Debug.LogWarning("No item prefabs assigned!");
            return;
        }
        // Check if there's already an item at the spawn point
        Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, 0.1f); // Adjust the radius as needed
        bool itemExists = false;
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Item")) // Assuming your spawned items have the "Item" tag
            {
                itemExists = true;
                break;
            }
        }
        // If there's no item at the spawn point, spawn a new one
        if (!itemExists)
        {
            // Choose a random item prefab from the list
            GameObject itemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];

            // Spawn the chosen item prefab at the position of the manually assigned spawn point
            if (spawnPoint != null)
            {
                Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
            }
        }
    }
}