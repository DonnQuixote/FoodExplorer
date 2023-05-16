using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public Vector3 worldPostion;
    public bool walkable;
    public int gridX;
    public int gridY;

    public int gCost;//distance from starting node;
    public int hCost;//distance from target node;
    public Node parent;

    public State state;

    [SerializeField]public float fuel = 10.0f;
    [SerializeField]public float temperature = 10.0f;
    [SerializeField]public float temperatureMax = 50.0f;

    int heapIndex;

    public Node(bool _walkable,Vector3 _worldPosition,int _gridX,int _gridY,State _state)
    {
        walkable = _walkable;
        worldPostion = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;
        state = _state;
    }
   
    public enum State{
        Unburnable,
        Ignited,
        Flammable,
        Consumed,
        NotStarted
    };

    public int fCost
    {
        get { return gCost + hCost; }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }

        set
        {
            heapIndex = value;
        }

    }

    
    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return compare;
    }

    public float GetTemperature()
    {
        return temperature;
    }

    public float GetFuel()
    {
        return fuel;
    }


    public void SetTemperature( float t)
    {
      temperature = t;
    }

    public void GetFuel(float f)
    {
      fuel = f;
    }


}
