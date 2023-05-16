using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] KitchenObjectsSO kitchenObjectsSO;
    private IKitchenObjectParent kitchenObjectParent;
    public Quaternion origionRatatoin;
    private void Awake()
    {
        origionRatatoin = transform.rotation;
    }

    public KitchenObjectsSO GetKitchenObjectsSO()
    {
        return kitchenObjectsSO;
    }

    public IKitchenObjectParent GetCleatCount()
    {
        return kitchenObjectParent;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if(this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }
        this.kitchenObjectParent = kitchenObjectParent;

        if (kitchenObjectParent.HasKitchenObject())
        {
            kitchenObjectParent.ClearKitchenObject();
        }
        kitchenObjectParent.SetKitchenObject(this);
        //Debug.Log(GetCleatCount());
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<KitchenObject>())
        {
            Debug.Log("OnCollisionEnter:  " + collision.gameObject.name);
            
        }
    }
    public void DestroySelf()
    {
        kitchenObjectParent.SetKitchenObject(null);
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectsSO kitchenObjectsSO,IKitchenObjectParent parent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectsSO.prefab);
        KitchenObject tempKitchenObjectsSO = kitchenObjectTransform.GetComponent<KitchenObject>();
        tempKitchenObjectsSO.SetKitchenObjectParent(parent);
        return tempKitchenObjectsSO;
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if(this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }

}
