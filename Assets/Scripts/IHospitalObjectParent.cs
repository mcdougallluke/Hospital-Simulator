using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHospitalObjectParent
{
    public Transform GetHospitalObjectFollowTransform();
    public void SetHospitalObject(HospitalObject hospitalObject);
    public HospitalObject GetHospitalObject();
    public void ClearHospitalObject();
    public bool HasHospitalObject();
}
