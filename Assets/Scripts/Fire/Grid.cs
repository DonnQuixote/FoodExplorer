using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Grid Instance { get; private set; }
    public bool onlyDisplayPathGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeWidth, gridSizeHeight;

    [SerializeField] FireManager fireManager;
    private Node beginSpawnFireNode;
    private bool startSpawnFireFlag = false;

    public event EventHandler<OnAnyNodeBeFiredArgs> OnAnyNodeBeFired;
    public class OnAnyNodeBeFiredArgs : EventArgs
    {
        public Node node;
    }

    //被点燃但是移除的列表
    List<Node> hasIgnited;

    //被加热但仍然没有被点燃
    List<Node> waitToIgnit;

    //所有历史上被点燃的节点
    List<Node> isIgnited;
    [SerializeField] private float temperatureSpeed = 20f;
    [SerializeField] private float extinguishFlameSpeed = 2.0f * 20;
    [SerializeField] private float fuelSpeed = 2.0f;

    int countMax = 20;
    int count;

    public struct IgnitedNode {
        public Node node;
        public List<Node> XYNeighbours;

        public IgnitedNode(Node _node,List<Node> list)
        {
            node = _node;
            XYNeighbours = list;
        }
    };
    List<IgnitedNode> listIgnitedNode;
    private void Awake()
    {
        
        listIgnitedNode = new List<IgnitedNode>();
        hasIgnited  = new List<Node>();
        waitToIgnit = new List<Node>();
        isIgnited = new List<Node>();
        Instance = this;
        nodeDiameter = nodeRadius * 2;
        gridSizeWidth = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeHeight = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    private void Start()
    {
        fireManager.OnSpawnFireManager += FireManager_OnSpawnFire;
        fireManager.OnPSDestroy += FireManager_OnPSDestroy;
    }

    private void FireManager_OnPSDestroy(object sender, FireManager.OnPSDestroyArgs e)
    {
        Node node = e.node;
        node.state = Node.State.Flammable;
        node.walkable = true;
        node.temperature = 0.0f;
        count--;
        //for(int i = 0; i < waitToIgnit.Count; i++)
        //{
        //    if(waitToIgnit[i] == node)
        //    {
        //        waitToIgnit.RemoveAt(i);
               
        //        break;
        //    }
        //}


        foreach(IgnitedNode ignitedNodeTemp in listIgnitedNode)
        {
            if(ignitedNodeTemp.node == node)
            {
                listIgnitedNode.Remove(ignitedNodeTemp);
                break;
            }
        }
    }

    private void FireManager_OnSpawnFire(object sender, FireManager.OnSpawnFireArgs e)
    {
        beginSpawnFireNode = e.node;
        beginSpawnFireNode.walkable = false;

        //begin fire propatation
        hasIgnited.Add(beginSpawnFireNode);
        isIgnited.Add(beginSpawnFireNode);
        startSpawnFireFlag = true;
        count++;
        IgnitedNode ignitedNode = new IgnitedNode(beginSpawnFireNode, GetNeighboursIfNotIgnited(beginSpawnFireNode));
        listIgnitedNode.Add(ignitedNode);
    }

    private void Update()
    {
        if (count > countMax)
        {
            startSpawnFireFlag = false;
        }
        if (startSpawnFireFlag)
        {
            //TransformFire();
            TransformFire2();
        }

        if (count == 0)
        {
            startSpawnFireFlag = false;
            startSpawnFireFlag = false;

        }
        //if (count < countMax)
        //{
        //    startSpawnFireFlag = true;
        //}


    }

    private void TransformFire2()
    {
        for (int i = 0; i < listIgnitedNode.Count; i++)
        {
            IgnitedNode ignitedNodeStruct = listIgnitedNode[i];
            Node node = ignitedNodeStruct.node;
            List<Node> neighbours = ignitedNodeStruct.XYNeighbours;
            for (int j = 0; j < neighbours.Count; j++)
            {
                    Node neibourNode = neighbours[j];
                    if (neibourNode.state == Node.State.Flammable && neibourNode.walkable)
                    {
                        float temperature = neibourNode.GetTemperature();
                        if (temperature < neibourNode.temperatureMax)
                        {
                            neibourNode.temperature += temperatureSpeed * Time.deltaTime * (count<4? (1):((Mathf.Sqrt(count))));
                        }
                        else
                        {
                            OnAnyNodeBeFired?.Invoke(this, new OnAnyNodeBeFiredArgs
                            {
                                node = neibourNode
                            });

                            count++;
                            neibourNode.walkable = false;

                            neibourNode.state = Node.State.Ignited;
                            listIgnitedNode.Add(new IgnitedNode(neibourNode, GetNeighboursIfNotIgnited(neibourNode)));
                        }
                    }
            }
        }
    }

    private void TransformFire()
    {
        Debug.Log("hasIgnited: " + hasIgnited.Count);
        Debug.Log("waitToIgnit: " + waitToIgnit.Count);

        //while (hasIgnited.Count != 0)
        //{
        int k = 0;
        int oldSize = hasIgnited.Count;
        for (k = 0; k < oldSize; k++)
        {
            List<Node> nei = new List<Node>();
            nei = GetNeighbours(hasIgnited[k]);

            for (int i = 0; i < nei.Count; i++)
            {
                if (nei[i].state == Node.State.Flammable && !waitToIgnit.Contains(nei[i]) &&
                    nei[i].walkable)
                {
                    waitToIgnit.Add(nei[i]);
                }
            }
        }
    //}

        //if (hasIgnited.Count == 0)
        //{
        //    startSpawnFireFlag = false;
        //}

        while(hasIgnited.Count != 0)
        {
            int waitToIgnitOldSize = waitToIgnit.Count;


            for (int j = 0; j < waitToIgnitOldSize; j++)
            {
                float temperature = waitToIgnit[j].GetTemperature();
                if (temperature < waitToIgnit[j].temperatureMax)
                {
                    waitToIgnit[j].temperature += temperatureSpeed * Time.deltaTime;
                }
                else
                {
                    OnAnyNodeBeFired?.Invoke(this, new OnAnyNodeBeFiredArgs
                    {
                        node = waitToIgnit[j]
                    });

                    count++;
                    hasIgnited.Add(waitToIgnit[j]);
                    isIgnited.Add(waitToIgnit[j]);
                    waitToIgnit[j].walkable = false;


                    waitToIgnit[j].state = Node.State.Ignited;

                    bool t = waitToIgnit.Remove(waitToIgnit[j]);
                    


                    if (t)
                    {
                        waitToIgnitOldSize--;
                        j--;
                        //if (j < -1) { j = -1; }
                    }

                    hasIgnited.Clear();
                }
            }
        }
    }


    public void ExtinguishFlame(Node node)
    {
        if (node.state != Node.State.Ignited) return;
    }

    public int MaxSize
    {
        get{
            return gridSizeWidth * gridSizeHeight;
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeWidth && checkY >=0 &&
                    checkY < gridSizeHeight)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    public List<Node> GetNeighboursIfNotIgnited(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (grid[checkX, checkY].state != Node.State.Flammable || !grid[checkX, checkY].walkable) continue;

                if (checkX >= 0 && checkX < gridSizeWidth && checkY >= 0 &&
                    checkY < gridSizeHeight)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeWidth, gridSizeHeight];

        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 -
            Vector3.forward * gridWorldSize.y / 2;

        for(int x =0;x < gridSizeWidth; x++)
        {
            for(int y =0;y < gridSizeHeight; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) +
                    Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask)); //
                grid[x, y] = new Node(walkable, worldPoint,x,y,Node.State.Flammable);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt(percentX * ( gridSizeWidth - 1));
        int y = Mathf.RoundToInt(percentY * (gridSizeHeight - 1));
        return grid[x, y];
    }

    public List<Node> path;
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (onlyDisplayPathGizmos)
        {
            if (path != null)
            {
                foreach(Node n in path)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.worldPostion, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
        else
        {
            if (grid != null)
            {
                foreach (Node n in grid)
                {
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                    if (path != null)
                    {
                        if (path.Contains(n))
                        {
                            Gizmos.color = Color.black;
                        }
                    }
                    Gizmos.DrawCube(n.worldPostion, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
    }

}
