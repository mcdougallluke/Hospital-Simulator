using UnityEngine;

public class DontDestroyUnique : MonoBehaviour
{
    // Static reference to the instance
    private static DontDestroyUnique instance;

    void Awake()
    {
        // If there is no instance yet, set this as the instance and don't destroy it
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // If an instance already exists and it's not this one, destroy this one
        /*else if (instance != this)
        {
            Destroy(gameObject);
        }*/
    }
}
