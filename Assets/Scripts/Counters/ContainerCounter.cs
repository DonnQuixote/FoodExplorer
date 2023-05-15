using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectsSO kitchenObjectsSO;
    public event EventHandler OnPlayerGrabbedObject;
    public override void Interaction(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //Debug.LogFormat("Interaction: {0}", transform.name);
            KitchenObject.SpawnKitchenObject(kitchenObjectsSO, player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
        
    }

    public override void Interaction_Cut(Player player)
    {
        

    }
}
