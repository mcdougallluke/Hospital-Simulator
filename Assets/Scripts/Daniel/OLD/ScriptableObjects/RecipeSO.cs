using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    public List<HospitalObjectSO> hospitalObjectsSOList;
    public string recipeName;
}
