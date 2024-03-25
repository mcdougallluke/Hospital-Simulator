using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance { get; private set; }
    
    // Public list to assign in the Unity Editor
    public List<Transform> initialPoints;

    private Dictionary<Transform, bool> pointAvailability = new Dictionary<Transform, bool>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Automatically register the points added in the list through the Unity Editor
            foreach (var point in initialPoints)
            {
                RegisterPoint(point);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterPoint(Transform point)
    {
        if (!pointAvailability.ContainsKey(point))
        {
            pointAvailability.Add(point, true); // Points are available by default
            Debug.Log($"Registered point: {point.name}");

        }
    }

    public bool IsPointAvailable(Transform point)
    {
        if (pointAvailability.TryGetValue(point, out bool isAvailable))
        {
            return isAvailable;
        }
        return false; // Consider unregistered points as unavailable
    }

    public void SetPointAvailability(Transform point, bool isAvailable)
    {
        if (pointAvailability.ContainsKey(point))
        {
            pointAvailability[point] = isAvailable;
            Debug.Log($"Point {point.name} availability set to: {isAvailable}");
        }
    }

}
