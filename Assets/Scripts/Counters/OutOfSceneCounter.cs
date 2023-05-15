using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfSceneCounter : BaseCounter
{
    public static OutOfSceneCounter Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }
    public override void Interaction(Player player)
    {
         
    }

    public override void Interaction_Cut(Player player)
    {

    }
}
