using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    
    [SerializeField] private Transform topOfCounterTransform;
    public static event EventHandler OnAnyObjectPlacedHere;

    public static void ResetStaticData()
    {
        OnAnyObjectPlacedHere = null;
    }
    private KitchenObject kitchenObject;
    public virtual void Interaction(Player player)
    {
        Debug.LogError("BaseCounter.Interact");
    }

    public virtual void Interaction_Cut(Player player)
    {
        Debug.LogError("BaseCounter.Interaction_Cut");
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return topOfCounterTransform;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if(kitchenObject != null)
        {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
}
