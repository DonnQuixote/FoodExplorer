using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlateIconUI : MonoBehaviour
{
    [SerializeField] private Image image;

    public void SetKitchenObjectSO(KitchenObjectsSO kitchenObjectSO)
    {
        image.sprite = kitchenObjectSO.sprite;
    }
}
