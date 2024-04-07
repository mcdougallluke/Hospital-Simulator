using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }
    
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
}