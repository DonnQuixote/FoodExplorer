using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlateCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    [SerializeField] private KitchenObjectsSO plateKitchenObjectSO;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4.0f;
    private int plateSpawnedAmount;
    private int plateSpawnAmountMax = 4;
    public override void Interaction(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //player is empty handed
            if(plateSpawnedAmount > 0)
            {
                //there's at least one plate here
                plateSpawnedAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interaction_Cut(Player player)
    {

    }

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if(spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;
            if(plateSpawnedAmount < plateSpawnAmountMax)
            {
                plateSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
