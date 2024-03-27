using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{
    public HospitalObjectSO input;
    public HospitalObjectSO output;
    public int cuttingProgressMax;
}
