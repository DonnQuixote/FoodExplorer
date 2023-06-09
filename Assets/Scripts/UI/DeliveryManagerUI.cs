using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeCompleted += Delivery_OnRecipeCompleted;
        DeliveryManager.Instance.OnSpawnRecipe += Delivery_OnSpawnRecipe;

        UpdateVisual();
    }

    private void Delivery_OnSpawnRecipe(object sender, System.EventArgs e)
    {
        UpdateVisual();

    }

    private void Delivery_OnRecipeCompleted(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach(Transform child in container)
        {
            if (child == recipeTemplate) continue;
            else Destroy(child.gameObject);
        }

        foreach (RecipeSO recipeSO in (DeliveryManager.Instance.GetWaitingRecipeSOList()))
        {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }
    }
}
