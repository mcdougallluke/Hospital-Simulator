using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (player.HasHospitalObject())
        {
            // Only accepts plates
            player.GetHospitalObject().DestroySelf();
        }
    }
}