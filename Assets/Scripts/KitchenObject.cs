using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] KitchenObjectsSO kitchenObjectsSO;
    private ClearCount clearCount;

    public KitchenObjectsSO GetKitchenObjectsSO()
    {
        return kitchenObjectsSO;
    }

    public ClearCount GetCleatCount()
    {
        return clearCount;
    }

    public void SetCleatCount(ClearCount clearCount)
    {
        if(this.clearCount != null)
        {
            this.clearCount.ClearKitchenObject();
        }
        this.clearCount = clearCount;

        if (clearCount.HasKitchenObject())
        {
            Debug.LogError("�Ѿ�����һ��KitchenObject,��̨�ϴ�������");
        }
        clearCount.SetKitchenObject(this);

        transform.parent = clearCount.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
}
