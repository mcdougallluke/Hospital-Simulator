using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }
    public Text[] roomTexts; 
    public GameObject[] timerBars; 
    public float proximityThreshold = 5.0f;
    public List<Transform> npcs; 
    public List<Transform> initialRooms;
    private Dictionary<Transform, bool> roomAvailability = new Dictionary<Transform, bool>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            foreach (var room in initialRooms)
            {
                RegisterRoom(room);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < roomTexts.Length; i++)
        {
            roomTexts[i].text = "Empty";
        }
    }
    
    private void Update()
    {
        UpdateRoomStatus();
    }

    private void UpdateRoomStatus()
    {
        for (int i = 0; i < roomTexts.Length; i++)
        {
            Transform room = initialRooms[i];
            bool isRoomAvailable = IsRoomAvailable(room);
            bool isNpcClose = false;

            foreach (var npc in npcs)
            {
                if (IsNpcCloseToRoom(room, npc))
                {
                    isNpcClose = true;
                    roomTexts[i].text = "Occupied";
                    timerBars[i].SetActive(true);
                    StartTimerBarShrink(timerBars[i], 10f); // Start shrinking the timer bar over 10 seconds
                    break; // Ensure only one NPC starts the timer per room
                }
            }

            if (!isNpcClose)
            {
                roomTexts[i].text = "Empty";
                timerBars[i].SetActive(false);
            }
        }
    }

    private void StartTimerBarShrink(GameObject timerBar, float duration)
    {
        StartCoroutine(ShrinkTimerBar(timerBar, duration));
    }

    private IEnumerator ShrinkTimerBar(GameObject timerBar, float duration)
    {
        RectTransform barTransform = timerBar.GetComponent<RectTransform>();
        Vector2 originalSize = barTransform.sizeDelta;

        // Set pivot to the left side of the bar, so it shrinks towards the left
        barTransform.pivot = new Vector2(0, 0.5f);

        float currentTime = 0;
        while (currentTime < duration)
        {
            float scale = 1 - (currentTime / duration);
            barTransform.sizeDelta = new Vector2(originalSize.x * scale, originalSize.y);
            currentTime += Time.deltaTime;
            yield return null;
        }

        barTransform.sizeDelta = new Vector2(0, originalSize.y); // Ensure it's fully shrunk
        timerBar.SetActive(false); // Optionally hide the bar completely after shrinking
    }
    
    public void RegisterRoom(Transform point)
    {
        if (!roomAvailability.ContainsKey(point))
        {
            roomAvailability.Add(point, true);
            Debug.Log($"Registered point: {point.name}");
        }
    }

    public bool IsRoomAvailable(Transform point)
    {
        if (roomAvailability.TryGetValue(point, out bool isAvailable))
        {
            return isAvailable;
        }
        return false;
    }

    public void SetRoomAvailability(Transform room, bool isAvailable)
    {
        if (roomAvailability.ContainsKey(room))
        {
            roomAvailability[room] = isAvailable;
            Debug.Log($"Room {room.name} availability set to: {isAvailable}");
        }
    }

    public void ResetAllRoomsAvailability()
    {
        var keys = new List<Transform>(roomAvailability.Keys);

        foreach (var room in keys)
        {
            roomAvailability[room] = true;
            Debug.Log($"Room {room.name} availability reset to: true");
        }
    }

    public bool IsNpcCloseToRoom(Transform room, Transform npc)
    {
        // Calculate the distance between the room and the NPC
        float distance = Vector3.Distance(room.position, npc.position);

        // Check if the distance is less than or equal to the threshold
        return distance <= proximityThreshold;
    }

    public void RegisterNPC(Transform npc)
    {
        if (!npcs.Contains(npc))
        {
            npcs.Add(npc);
            Debug.Log($"NPC Registered: {npc.name}");
        }
    }

    public void UnregisterNPC(Transform npc)
    {
        if (npcs.Contains(npc))
        {
            npcs.Remove(npc);
            Debug.Log($"NPC Unregistered: {npc.name}");
        }
    }
}