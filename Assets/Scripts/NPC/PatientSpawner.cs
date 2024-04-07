using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PatientSpawner : MonoBehaviour
{
    public Transform despawnPoint;
    public GameObject patientPrefab; // Assign in the inspector
    public Transform spawnPoint; // Assign a transform as the spawn point in the inspector
    public SpellingMinigame spellingMinigame; // Assign in the inspector
    public FetchMinigame fetchMinigame; // Assign in the inspector
    public Score scoreScript; // Assign in the inspector
    public Transform waitingRoom; // Assign in the inspector
    public Transform[] examRooms; // Assign in the inspector

    private float spawnCooldown = 2.0f;
    private int maxPatientCount = 10;


    void Start()
    {
        StartCoroutine(SpawnPatientRoutine());
    }

    private IEnumerator SpawnPatientRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnCooldown);
            SpawnPatientIfPossible();
        }
    }

    private void SpawnPatientIfPossible()
    {
        if (GameObject.FindObjectsOfType<PatientAI>().Length < maxPatientCount)
        {
            SpawnPatient();
        }
    }

    private void SpawnPatient()
    {
        // Instantiate the NPC at the spawn point
        GameObject patient = Instantiate(patientPrefab, spawnPoint.position, spawnPoint.rotation);

        // Get the PatientAI component and assign the references
        PatientAI patientAI = patient.GetComponent<PatientAI>();
        if (patientAI != null)
        {
            patientAI.despawnPoint = despawnPoint;
            patientAI.spellingMinigame = spellingMinigame;
            patientAI.fetchMinigame = fetchMinigame;
            patientAI.scoreScript = scoreScript;
            patientAI.waitingRoom = waitingRoom;
            patientAI.examRooms = examRooms;
        }

        // New logic: Get all child GameObjects (skins) of the instantiated patient
        List<GameObject> childSkins = new List<GameObject>();
        foreach (Transform child in patient.transform)
        {
            if (child.gameObject.tag == "Skin")
            {
                childSkins.Add(child.gameObject);
            }
        }

        // Disable all skins initially
        childSkins.ForEach(child => child.SetActive(false));

        // If there are any child skins, pick one at random and enable it
        if (childSkins.Count > 0)
        {
            int skinIndex = Random.Range(0, childSkins.Count);
            childSkins[skinIndex].SetActive(true);
        }
    }
}