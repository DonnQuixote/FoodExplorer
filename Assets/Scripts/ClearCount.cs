using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCount : MonoBehaviour
{

    [SerializeField] private KitchenObjectsSO kitchenObjectsSO;
    [SerializeField] private Transform topOfCounterTransform;
    [SerializeField] private ClearCount secondClearCounter;
    private KitchenObject kitchenObject;
    [SerializeField] private bool Testing;


    
    private void Update()
    {
        if(Testing && Input.GetKeyDown(KeyCode.T) && kitchenObject!=null)
        {
            kitchenObject.SetCleatCount(secondClearCounter);
        }
    }
    public void Interaction()
    {
        if(kitchenObject == null)
        {
            Debug.LogFormat("Interaction: {0}", transform.name);
            Transform kitchenObjectTransform = Instantiate(kitchenObjectsSO.prefab, topOfCounterTransform);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetCleatCount(this);
        }
        else
        {
            //Give the object to the player.
            Debug.Log(kitchenObject.GetCleatCount());
            Debug.Log("柜台上已经存在物品");
        }
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return topOfCounterTransform;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
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
