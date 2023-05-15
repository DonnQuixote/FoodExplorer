using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter,IHasProgress
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSO;
    [SerializeField] private OutOfSceneCounter outOfSceneClearCount;
    private CuttingRecipeSO[] beCutRecipeSO;


    private int  cuttingProcess;

    public event EventHandler OnCut;
    public static event EventHandler OnAnyCut;
    public event EventHandler<IHasProgress.OnProcessChangedEventArgs> OnProcessChanged;

    private void Awake()
    {
        beCutRecipeSO = cuttingRecipeSO;
    }

    private void Start()
    {
        outOfSceneClearCount = OutOfSceneCounter.Instance;

    }
    public class OnProcessChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }


    public override void Interaction(Player player)
    {
        //Debug.Log("CuttingCounter");
        if (!HasKitchenObject())
        {
            //There is not anything on the KitchenCounter
            if (player.HasKitchenObject())
            {
                //player is carrying something
                if (HasCuttingRecipeObject(player.GetKitchenObject().GetKitchenObjectsSO()))
                {
                    //Playing is carrying something that can be cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProcess = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectsSO());
                    OnProcessChanged?.Invoke(this, new IHasProgress.OnProcessChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProcess / cuttingRecipeSO.cuttingProgressMax
                    }) ;
                }else if (HasBeCuttRecipeObject(player.GetKitchenObject().GetKitchenObjectsSO()))
                {
                    //player is carrying something has be cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }
                //player.GetKitchenObject().SetKitchenObjectParent(this);

            }
            else
            {
                //player is not carrying
            }
        }
        else
        {
            //There is something on the KitchenCounter
            if (!player.HasKitchenObject())
            {
                //player is not carrying
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
            else
            {
                //player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
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
                    //if (!HasCuttingRecipeObject(player.GetKitchenObject().GetKitchenObjectsSO()))
                    //{
                        
                    //}
                    cuttingProcess = 0;
                    player.GetKitchenObject().SetKitchenObjectParent(outOfSceneClearCount);
                    this.GetKitchenObject().SetKitchenObjectParent(player);
                    outOfSceneClearCount.GetKitchenObject().SetKitchenObjectParent(this);
                }
            }
        }
    }
     
    public override void Interaction_Cut(Player player)
    {
        if (HasKitchenObject() && HasCuttingRecipeObject(GetKitchenObject().GetKitchenObjectsSO()))
        {
            //There is a kitchenObject here AND it can be cut
            cuttingProcess++;
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectsSO());

            if (cuttingProcess <= cuttingRecipeSO.cuttingProgressMax)
            {
                OnCut?.Invoke(this, EventArgs.Empty);
            }

            //Debug.Log(OnAnyCut.GetInvocationList().Length);
            OnAnyCut?.Invoke(this, EventArgs.Empty);
          

            OnProcessChanged?.Invoke(this, new IHasProgress.OnProcessChangedEventArgs
            {
                progressNormalized = (float)cuttingProcess / cuttingRecipeSO.cuttingProgressMax
            });
            if(cuttingRecipeSO.cuttingProgressMax <= cuttingProcess)
            {
                KitchenObjectsSO cutKitchenObjectSO = FindKitchenObjectSO(GetKitchenObject().GetKitchenObjectsSO());
                GetKitchenObject().DestroySelf();
                //Debug.Log("has changed the origion thing");
                KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
            } 
        }
    }

    private bool HasCuttingRecipeObject(KitchenObjectsSO input)
    {
        
        //return FindKitchenObjectSO(input) == null ? false : true;
        return FindKitchenObjectSO(input) != null;
    }

    private KitchenObjectsSO FindKitchenObjectSO(KitchenObjectsSO input)
    {
        CuttingRecipeSO cuttingRecipeSOTemp = GetCuttingRecipeSOWithInput(input);
        if (cuttingRecipeSOTemp != null)
        {
            return cuttingRecipeSOTemp.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectsSO inputKitchenObjectSO)
    {
        //Debug.LogFormat("input is {0}", inputKitchenObjectSO.name);

        foreach (CuttingRecipeSO cutting in cuttingRecipeSO)
        {
            if (cutting.input == inputKitchenObjectSO)
            {
                //Debug.LogFormat("output is {0}",cutting.name);
                return cutting;
            }
        }

        return null;
    }


    private bool HasBeCuttRecipeObject(KitchenObjectsSO output)
    {
        
        //return FindKitchenObjectSO(input) == null ? false : true;
        return FindKitchenObjectSOBeCut(output) != null;
    }

    private KitchenObjectsSO FindKitchenObjectSOBeCut(KitchenObjectsSO output)
    {
        CuttingRecipeSO cuttingRecipeSOTemp = GetBeCutRecipeSOWithOutPut(output);
        if (cuttingRecipeSOTemp != null)
        {
            return cuttingRecipeSOTemp.input;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetBeCutRecipeSOWithOutPut(KitchenObjectsSO outputKitchenObjectSO)
    {
        //Debug.LogFormat("input is {0}", inputKitchenObjectSO.name);

        foreach (CuttingRecipeSO beCut in beCutRecipeSO)
        {
            if (beCut.output == outputKitchenObjectSO)
            {
                //Debug.LogFormat("output is {0}",cutting.name);
                return beCut;
            }
        }

        return null;
    }
}
