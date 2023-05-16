using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RecipeSO : ScriptableObject
{
    public List<KitchenObjectsSO> kitchenObjectsSOList;
    public string recipeName;

    //meat five,other two
    public int score = 0;
}
