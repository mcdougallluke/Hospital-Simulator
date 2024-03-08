using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IHospitalObjectParent
{
    [SerializeField] private Transform counterTopPoint;

    private HospitalObject hospitalObject;

    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact();");
    }
    public virtual void InteractAlternate(Player player)
    {
        Debug.LogError("BaseCounter.InteractAlternate();");
    }
    public Transform GetHospitalObjectFollowTransform()
    {
        return counterTopPoint;
    }
    public void SetHospitalObject(HospitalObject hospitalObject)
    {
        this.hospitalObject = hospitalObject;
    }
    public HospitalObject GetHospitalObject()
    {
        return hospitalObject;
    }
    public void ClearHospitalObject()
    {
        hospitalObject = null;
    }
    public bool HasHospitalObject()
    {
        return hospitalObject != null;
    }
}