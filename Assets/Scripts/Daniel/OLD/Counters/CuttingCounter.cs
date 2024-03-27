using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }

    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;
    public override void Interact(Player player)
    {
        if (!HasHospitalObject())
        {
            // There is no HospitalObject here
            if (player.HasHospitalObject())
            {
                // Player is carrying something
                if (HasRecipeWithInput(player.GetHospitalObject().GetHospitalObjectSO()))
                {
                    // Player is carrying something that can be cut
                    player.GetHospitalObject().SetHospitalObjectParent(this);
                    cuttingProgress = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetHospitalObject().GetHospitalObjectSO());

                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                    {
                        progressNormalized = (float) cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    }) ;
                }
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
    public override void InteractAlternate(Player player)
    {
        if (HasHospitalObject() && HasRecipeWithInput(GetHospitalObject().GetHospitalObjectSO()))
        {
            // There exists a HospitalObject here AND it can be cut
            cuttingProgress++;

            OnCut?.Invoke(this,EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetHospitalObject().GetHospitalObjectSO());

            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                HospitalObjectSO outputHospitalObjectSO = GetOutputForInput(GetHospitalObject().GetHospitalObjectSO());
                GetHospitalObject().DestroySelf();

                HospitalObject.SpawnHospitalObject(outputHospitalObjectSO, this);
            }
        }
    }

    private bool HasRecipeWithInput(HospitalObjectSO inputHospitalObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputHospitalObjectSO);
        return cuttingRecipeSO != null;
    }

    private HospitalObjectSO GetOutputForInput(HospitalObjectSO inputHospitalObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputHospitalObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(HospitalObjectSO inputHospitalObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputHospitalObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}