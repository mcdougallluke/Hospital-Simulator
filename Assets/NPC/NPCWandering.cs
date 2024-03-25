using UnityEngine;
using UnityEngine.AI;

public class NPCWandering : MonoBehaviour
{
    public Transform initialPoint;
    public Transform[] optionalPoints;
    private NavMeshAgent agent;
    private bool isWaiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToInitialPoint();
    }

    void Update()
    {
        if (!isWaiting && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            
            CheckAndMoveToOptionalPoint();
                // NPC has arrived at the optional point
                isWaiting = true; // NPC will now wait here
                Debug.Log("NPC has arrived at the point and is now waiting.");
            

        }
    }

    void MoveToInitialPoint()
    {
        agent.destination = initialPoint.position;
    }

    void CheckAndMoveToOptionalPoint()
    {
        bool pointFound = false;
        foreach (var point in optionalPoints)
        {
            if (PointManager.Instance.IsPointAvailable(point))
            {
                pointFound = true;
                PointManager.Instance.SetPointAvailability(point, false); // Mark the point as unavailable
                agent.destination = point.position;
                Debug.Log($"Moving to point: {point.name}");
                return;
            }
        }
        
        if (!pointFound)
        {
            Debug.Log("No available points found.");
        }
    }


    void TryMoveToOptionalPointAgain()
    {
        foreach (var point in optionalPoints)
        {
            if (PointManager.Instance.IsPointAvailable(point))
            {
                CancelInvoke("TryMoveToOptionalPointAgain"); // Stop checking
                PointManager.Instance.SetPointAvailability(point, false);
                agent.destination = point.position;
                break;
            }
        }
    }
}
