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
    public ArrowInputMinigame arrowInputMinigame; // Assign in the inspector
    public VitalsMinigame vitalsMinigame; // Assign in the inspector
    public VaccineMinigame vaccineMinigame; // Assign in the inspector
    public Score scoreScript; // Assign in the inspector
    public Transform waitingRoom; // Assign in the inspector
    public Transform[] examRooms; // Assign in the inspector

    private float spawnCooldown = 0.1f;
    private int maxPatientCount = 6;

    private int patientsSpawned = 0;



    void Start()
    {
        StartCoroutine(SpawnPatientRoutine());
    }

    private IEnumerator SpawnPatientRoutine()
    {
        // Initial spawn of 6 patients with the normal cooldown
        for (int i = 0; i < 6; i++)
        {
            SpawnPatient();
            yield return new WaitForSeconds(spawnCooldown); // Initial fast spawn rate
        }

        // After the first 6 patients, increase the cooldown
        float extendedSpawnCooldown = 20.0f;

        while (true)
        {
            SpawnPatientIfPossible();
            yield return new WaitForSeconds(extendedSpawnCooldown); // Slower spawn rate after the initial rush
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
        patientsSpawned++; // Increment the count of spawned patients

        // Get the PatientAI component and assign the references
        PatientAI patientAI = patient.GetComponent<PatientAI>();
        if (patientAI != null)
        {
            patientAI.despawnPoint = despawnPoint;
            patientAI.spellingMinigame = spellingMinigame;
            patientAI.fetchMinigame = fetchMinigame;
            patientAI.arrowInputMinigame = arrowInputMinigame;
            patientAI.vitalsMinigame = vitalsMinigame;
            patientAI.vaccineMinigame = vaccineMinigame;
            patientAI.scoreScript = scoreScript;
            patientAI.waitingRoom = waitingRoom;
            patientAI.examRooms = examRooms;

            RoomManager.Instance.RegisterNPC(patientAI.transform);

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