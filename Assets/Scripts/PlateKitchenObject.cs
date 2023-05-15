using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlateKitchenObject : KitchenObject
{

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectsSO kitchenObjectsSO;
    }

    [SerializeField] private List<KitchenObjectsSO> validKitchenObjectSOList;
    private List<KitchenObjectsSO> kitchenObjectsSOList;

    private void Awake()
    {
        kitchenObjectsSOList = new List<KitchenObjectsSO>();
    }

    public void AddIngredient(KitchenObjectsSO kitchenObjectSO)
    {
        kitchenObjectsSOList.Add(kitchenObjectSO);
    }


    public bool TryAddIngredient(KitchenObjectsSO kitchenObjectSO)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            //Not a valid ingredient
            return false;
        }

        //gurantee that every kind of KitchenObjectSo only store for once
        if (kitchenObjectsSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        else
        {
            kitchenObjectsSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs{
                kitchenObjectsSO = kitchenObjectSO
            });
            return true;
        }
    }

    public List<KitchenObjectsSO> GetKitchenObjectSOList()
    {
        return kitchenObjectsSOList;
    }
}
