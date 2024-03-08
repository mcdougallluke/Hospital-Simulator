using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private HospitalObjectSO HospitalObjectSO;

    public override void Interact(Player player)
    {
        if (!HasHospitalObject())
        {
            // There is no HospitalObject here
            if (player.HasHospitalObject())
            {
                // Player is carrying something
                player.GetHospitalObject().SetHospitalObjectParent(this);
            }
            else
            {
                // Player not carrying anything
            }
        }
        else
        {
            // There is a HospitalObject here
            if (player.HasHospitalObject())
            {
                // Player is carrying something
            }
            else
            {
                // Player not carrying anything
                GetHospitalObject().SetHospitalObjectParent(player);
            }
        }
    }
}