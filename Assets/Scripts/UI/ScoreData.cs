using UnityEngine;

[CreateAssetMenu(fileName = "ScoreData", menuName = "ScriptableObjects/ScoreData", order = 1)]
public class ScoreData : ScriptableObject
{
    public int score;
    public int patientsSaved;
    public int patientsLost;
}
