using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalObject : MonoBehaviour
{
    [SerializeField] private HospitalObjectSO HospitalObjectSO;

    private IHospitalObjectParent hospitalObjectParent;

    public HospitalObjectSO GetHospitalObjectSO() { return HospitalObjectSO; }
    public void SetHospitalObjectParent(IHospitalObjectParent hospitalObjectParent)
    {
        if (this.hospitalObjectParent != null)
        {
            this.hospitalObjectParent.ClearHospitalObject();
        }


        this.hospitalObjectParent = hospitalObjectParent;

        if(hospitalObjectParent.HasHospitalObject())
        {
            Debug.LogError("The IHospitalObjectParent already has a HospitalObject!");
        }

        hospitalObjectParent.SetHospitalObject(this);


        transform.parent = hospitalObjectParent.GetHospitalObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IHospitalObjectParent GetHospitalObjectParent()
    {
        return hospitalObjectParent;
    }

    public void DestroySelf()
    {
        hospitalObjectParent.ClearHospitalObject();
        Destroy(gameObject);
    }

    public static HospitalObject SpawnHospitalObject(HospitalObjectSO hospitalObjectSO, IHospitalObjectParent hospitalObjectParent)
    {
        Transform hospitalObjectTransform = Instantiate(hospitalObjectSO.prefab);
        HospitalObject hospitalObject = hospitalObjectTransform.GetComponent<HospitalObject>();
        hospitalObject.SetHospitalObjectParent(hospitalObjectParent);

        return hospitalObject;
    }
}