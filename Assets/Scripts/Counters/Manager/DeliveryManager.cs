using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnSpawnRecipe; 
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;
    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> recipeList;

    [SerializeField]private float spawnRecipeTimer = 1.0f;
    [SerializeField]private float spawnRecipeTimerMax = 4.0f;
    [SerializeField]private int waitingRecipeMax = 4;
    [SerializeField]private int successedDelivery = 0;
    [SerializeField]private int successedScore = 50;

    private void Awake()
    {
        Instance = this;
        //spawnRecipeTimer = spawnRecipeTimerMax;
        recipeList = new List<RecipeSO>();
    }
    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if(spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (recipeList.Count <= waitingRecipeMax && KitchenGameObject.Instance.IsGamePlaying())
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeListSO[UnityEngine.Random.Range(0, recipeListSO.recipeListSO.Count)];

                OnSpawnRecipe?.Invoke(this, EventArgs.Empty);

                recipeList.Add(waitingRecipeSO);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for(int i =0; i < recipeList.Count; i++)
        {
            RecipeSO waitingRecipeSO = recipeList[i];

            if(waitingRecipeSO.kitchenObjectsSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                //Has the same number of ingredients
                bool plateContensMatchesRecipe = true;
                foreach(KitchenObjectsSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectsSOList)
                {
                    bool ingredientFound = false;
                    //Cycling through all ingredients in the Recipe
                    foreach(KitchenObjectsSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        //Cycling through all ingredients in the Plate
                        if(plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            //Ingredient matches
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        //This Recipe ingredient was not found on the Plate
                        plateContensMatchesRecipe = false;
                        break;
                    }
                }
                if (plateContensMatchesRecipe)
                {
                    //Player delivered the correct recipe!
                    Debug.Log("Player delivered the correct recipe!");
                    successedDelivery++;

                    successedScore += recipeList[i].score;

                    recipeList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    
                    return;
                }
               
            }
        }

        //No matches found!
        //Player did not deliver a correct recipe
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
        Debug.Log("Player did not deliver a correct recipe");
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return recipeList;
    }

    public int GetSuccessedDelivery()
    {
        return successedDelivery;
    }

    public int GetSuccessedScore()
    {
        return successedScore;
    }

    public int ChangeSuccessedScore(int cost)
    {
        if(successedScore - cost< 0)
        {
            return -1;
        }
        successedScore -= cost;
        return successedScore;
    }

}
