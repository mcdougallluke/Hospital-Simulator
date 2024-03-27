using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private HospitalObjectSO HospitalObjectSO;

    public override void Interact(Player player)
    {
        if (!player.HasHospitalObject())
        {
            // Player is not carrying anything
            HospitalObject.SpawnHospitalObject(HospitalObjectSO, player);
          
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            // Player is carrying something
        }
    }
}