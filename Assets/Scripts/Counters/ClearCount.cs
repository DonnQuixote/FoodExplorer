using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCount : BaseCounter
{

    
    [SerializeField] private KitchenObjectsSO kitchenObjectsSO;
    [SerializeField] private OutOfSceneCounter outOfSceneClearCount;
    //[SerializeField] private bool Testing;
    //[SerializeField] private ClearCount secondClearCounter;

    private void Start()
    {
        outOfSceneClearCount = OutOfSceneCounter.Instance;
    }

    private void Update()
    {
        /*if(Testing && Input.GetKeyDown(KeyCode.T) && kitchenObject!=null)
        {
            kitchenObject.SetKitchenObjectParent(secondClearCounter);
        }*/
    }
    public  override void Interaction(Player player)
    {
        if (!HasKitchenObject())
        {
            //There is not anything on the KitchenCounter
            if (player.HasKitchenObject())
            {
                //player is carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                //player is not carrying
            }
        }
        else{
            //There is something on the KitchenCounter
            if (!player.HasKitchenObject())
            {
                //player is not carrying
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
            else
            {
                //player is carrying something
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //player is carrying a  plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectsSO()))
                    {
                        //the top of the ClearCounter is what we want the slices
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    //player is carrying something but not a plate
                    if(GetKitchenObject().TryGetPlate(out  plateKitchenObject)){
                        //Counter is holding a Plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectsSO())){
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                    else
                    {
                        player.GetKitchenObject().SetKitchenObjectParent(outOfSceneClearCount);
                        this.GetKitchenObject().SetKitchenObjectParent(player);
                        outOfSceneClearCount.GetKitchenObject().SetKitchenObjectParent(this);
                    }
                }
            }
        }
    }


    public override void Interaction_Cut(Player player)
    {


    }
}
