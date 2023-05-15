using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class PathFinding : MonoBehaviour
{
    public Transform seeker, target;
    Grid grid;
    private int CostAxis = 10;
    private int CostDiagonal = 14;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //FindPath(seeker.position, target.position);
        }
        FindPath(seeker.position, target.position);

    }

    void FindPath(Vector3 startPos,Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        //List<Node> openSet = new List<Node>();
        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        
        HashSet<Node> clostSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            //Node node = openSet[0];
            ////node in open with the lowest f_cost;
            //for (int i = 1; i < openSet.Count; i++)
            //{
            //    if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
            //    {
            //        if (openSet[i].hCost < node.hCost) node = openSet[i];
            //    }
            //}
            //openSet.Remove(node);
            //clostSet.Add(node);


            Node node = openSet.RemoveFirst();
            clostSet.Add(node);

            if (node == targetNode)
            {
                sw.Stop();
                //print("Path found£º" + sw.ElapsedMilliseconds + "ms");
                RetracePath(startNode, targetNode);
                return;
            }

            foreach(Node neighbour in grid.GetNeighbours(node))
            {
                if(!neighbour.walkable || clostSet.Contains(neighbour))
                {
                    continue;
                }


                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if(newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(startNode);
        path.Reverse();
        grid.path = path;
    }

    //Vector3[] RetracePath(Node startNode,Node endNode)
    //{
    //    List<Node> path = new List<Node>();
    //    Node currentNode = endNode;

    //    while(currentNode!= endNode)
    //    {
    //        path.Add(currentNode);
    //        currentNode = currentNode.parent;
    //    }
    //    Vector3[] waypoints = SimplyfyPath(path);
    //    Array.Reverse(waypoints);
    //    return waypoints;
    //}

    Vector3[] SimplyfyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPostion);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    //the digital of 10 and 14 are the cost of walking along the axis and the diagonal
    int GetDistance(Node nodeA,Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY) return CostDiagonal * dstY + CostAxis * (dstX - dstY);
        return CostDiagonal * dstX + CostAxis * (dstY - dstX);
    }
}
