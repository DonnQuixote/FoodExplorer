using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCounter : BaseCounter
{
    public static ShopCounter Instance { get; private set; }
    public event EventHandler OnInteractionWithShop;

    private void Awake()
    {
        Instance = this;
    }
    public override void Interaction(Player player)
    {
        Debug.Log("interaction with shop");
        Time.timeScale = 0;
        OnInteractionWithShop?.Invoke(this, EventArgs.Empty);
    }

    public override void Interaction_Cut(Player player)
    {

    }
}
