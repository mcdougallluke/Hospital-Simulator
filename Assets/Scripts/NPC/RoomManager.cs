using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }
    public Text[] roomTexts; // Assign this array in the inspector with your UI Texts
    public float proximityThreshold = 5.0f;
    public List<Transform> npcs; // Assign NPC transforms in the Unity Editor



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

    void UpdateRoomStatus()
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
                }
            }

            // Update the room availability if no NPC is close
            if (!isNpcClose)
            {
                roomTexts[i].text = "Empty";
            }

        }
    }


    // Public list to assign in the Unity Editor
    public List<Transform> initialRooms;
    private Dictionary<Transform, bool> roomAvailability = new Dictionary<Transform, bool>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);

            // Automatically register the points added in the list through the Unity Editor
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

    public void RegisterRoom(Transform point)
    {
        if (!roomAvailability.ContainsKey(point))
        {
            roomAvailability.Add(point, true); // Points are available by default
            Debug.Log($"Registered point: {point.name}");

        }
    }

    public bool IsRoomAvailable(Transform point)
    {
        if (roomAvailability.TryGetValue(point, out bool isAvailable))
        {
            return isAvailable;
        }
        return false; // Consider unregistered points as unavailable
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
        // Create a copy of the dictionary keys to avoid modifying the collection while iterating
        var keys = new List<Transform>(roomAvailability.Keys);

        foreach (var room in keys)
        {
            roomAvailability[room] = true; // Reset all rooms to available
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