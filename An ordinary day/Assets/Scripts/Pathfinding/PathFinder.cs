using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Behaviour searching the shortest path given a grid of obstacles (collider scanner)
/// </summary>
public class PathFinder : MonoBehaviour
{
    private const float DefaultWeight = 1f;

    [SerializeField]
    private ColliderScanner _colliderScanner; // obstacle grid

    [Header("Parameters")]
    [SerializeField]
    [Tooltip("If true, check also the neighbours in diagonal direction")]
    private bool _checkDiagonalNeighbours = true;
    [SerializeField]
    private WeightedElement[] _weightedElements;

    [Serializable]
    private class WeightedElement
    {
        public string ObjectTag;
        [Range(1f,5f)]
        public float Weight = 3f;
    }

    [Header("Debug")]
    [SerializeField]
    private Transform _start;
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private bool _showDebug;
    [SerializeField]
    private Color _pathColor;

    private List<Vector2> _debugPath;
    private WorldGrid<Collider2D> CollidersGrid => _colliderScanner.ScanResult;
    private Collider2D[] _collidersToIgnore;

    /// <summary>
    /// Finds the shortest path from start to final.
    /// Return a list of position, starting by the starting position.
    /// Return null if any path could be found or if the target is unreachable.
    /// </summary>
    /// <returns>The shortest path.</returns>
    /// <param name="start">Start.</param>
    /// <param name="final">Final.</param>
    /// // todo Optimize
    public List<Vector2> FindShortestPath(Vector2 start, Vector2 final, Collider2D[] collidersToIgnore = null)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        Debug.Log("Search path from " + start + " to " + final);
        _collidersToIgnore = collidersToIgnore;
        var finalNode = new Node(CollidersGrid.GetGridIndex(final), null);
        if (CollidersGrid[finalNode.Position.x, finalNode.Position.y])
        {
            Debug.LogError("Target is unreachable.");
            return null;
        }
        // Preparation of the algorithm
        var startNode = new Node(CollidersGrid.GetGridIndex(start), null);
        var closeList = new List<Vector2Int>();
        var openList = new List<Node>();
        AddNodeInOpenList(openList, startNode);
        int count = 0;
        Debug.Log("Search path from node: " + startNode + " to " + finalNode);
        while (openList.Count > 0)
        {
            // Pick up the node with the lowest heuristic and remove it from the open list
            var node = openList[0];
            openList.Remove(node);
            // we reached the final position: return the path and stop algorithm
            if (node.Position == finalNode.Position)
            {
                Debug.Log(string.Format("Path found in {0} iterations !", count));
                watch.Stop();
                Debug.Log("Path finding computation time: " + (watch.ElapsedMilliseconds / 1000f));
                return GetWorldCoordinatePath(node);
            }
            // We check all the neighbours of the given node
            foreach (var neighbour in GetReachableNeighbours(node))
            {
                // already in closed list, dont take into account
                if (closeList.Contains(neighbour.Position))
                    continue;

                // compute cost
                neighbour.Cost = ComputeCost(neighbour, node);

                // pass if already in opened list and its cost is superior
                var neighBourInOpenList = openList.GetNodeByPosition(neighbour);
                if (neighBourInOpenList != null)
                {
                    // less interesting node, so we just skip this one
                    if (neighBourInOpenList.Cost <= neighbour.Cost) 
                        continue;
                    else // we remove the previous version of the node from the opened list
                        openList.Remove(neighBourInOpenList);
                }
                neighbour.Heuristic = ComputeHeuristic(neighbour, finalNode);
                AddNodeInOpenList(openList, neighbour);
                //openList.Add(neighbour);
            }
            // We add the node to the close if he doesnt exist already
            if (!closeList.Contains(node.Position))
                closeList.Add(node.Position);
            count++;
        }
        // couldnt find any path
        Debug.LogError("Couldnt find any path from: " + start + " to " + final);
        return null;
    }


    private void AddNodeInOpenList(List<Node> openList, Node node)
    {
        if (openList == null)
            return;
        float currentHeuristic;
        for (int i = 0; i < openList.Count; i++)
        {
            currentHeuristic = openList[i].Heuristic;
            if (node.Heuristic < currentHeuristic)
            {
                openList.Insert(i, node);
                return;
            }
        }
        openList.Add(node);
    }


    // Get the neighbours of the given node
    private List<Node> GetReachableNeighbours(Node node)
    {
        var neighbours = new List<Node>();
        var nodeX = node.Position.x; // sugar
        var nodeY = node.Position.y; // sugar

        var minY = Mathf.Max(nodeY - 1, 0);
        var maxY = Mathf.Min(nodeY + 1, CollidersGrid.Ny - 1);

        for (int x = Mathf.Max(nodeX - 1, 0);
                 x <= Mathf.Min(nodeX+ 1,CollidersGrid.Nx-1); x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                // not reachable or same node as origin
                if (!_checkDiagonalNeighbours && (x != nodeX && y != nodeY))
                    continue;
                if ((nodeX == x && nodeY == y) || HasCollider(x,y))
                    continue;
                neighbours.Add(new Node(new Vector2Int(x, y), node));
            }
        }
        return neighbours;
    }

    /// <summary>
    /// Return true if the given position has a collider that shouldnt be ignored.
    /// </summary>
    private bool HasCollider(int x, int y)
    {
        if (CollidersGrid[x, y] == null // no collider
                                        // if the collider has to be ignored, return false                            
            || CollidersGrid[x,y].isTrigger 
            || (_collidersToIgnore != null && _collidersToIgnore.Contains(CollidersGrid[x, y]))
             )
            return false;

        return true;
    }


    /// <summary>
    /// Reconstructs the path going to a given node, using its parents
    /// Start with the start position
    /// </summary>
    /// <returns>The world coordinate path.</returns>
    /// <param name="node">Node.</param>
    private List<Vector2> GetWorldCoordinatePath(Node node)
    {
        var path = new List<Vector2>();
        while (node.Parent != null)
        {
            path.Add(CollidersGrid.GetWordCoordinate(node.Position));
            node = node.Parent;
        }
        path.Reverse(); // to start with the start position
        return path;
    }


    #region Costs
    private float ComputeHeuristic(Node node, Node target)
    {
        return node.Cost + node.GetDistanceFrom(target) / GetWeight(node);
    }


    private float ComputeCost(Node node, Node parent)
    {
        return parent.Cost + node.GetDistanceFrom(parent) / GetWeight(node);
    }


    private float GetWeight(Node node)
    {
        // no weighted elements defined, return default weight
        if (_weightedElements == null || _weightedElements.Length == 0)
            return DefaultWeight;
        var nodeCollider = CollidersGrid[node.Position.x, node.Position.y];
        if (nodeCollider != null && nodeCollider.isTrigger)
        {
            // If the collider found has a tag defined in the weighted element list
            var weightedElement = _weightedElements.FirstOrDefault((x) => x.ObjectTag.Equals(nodeCollider.tag));
            if (weightedElement != null)
                return weightedElement.Weight;
        }
        return DefaultWeight;
    }


    #endregion

    public void SetColliderScanner(ColliderScanner colliderScanner)
    {
        _colliderScanner = colliderScanner;
    }

    #region Debug

    public void FindPathDebug()
    {
        // Debug
        _debugPath = FindShortestPath(_start.position, _target.position);
        Debug.Log(_debugPath);
    }

    private void OnDrawGizmos()
    {
        // area
        if (!_showDebug)
            return;
        // Draw Path
        Gizmos.color = _pathColor;
        if (_debugPath != null)
        {
            foreach (var point in _debugPath)
                Gizmos.DrawCube(point, Vector3.one * 0.5f);
        }
    }
    #endregion
}
