using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TrashCounter : BaseCounter
{

    public static event EventHandler OnAnyObjectTrashed;

    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }
    public override void Interaction(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }

    public override void Interaction_Cut(Player player)
    {
       
    }
}
