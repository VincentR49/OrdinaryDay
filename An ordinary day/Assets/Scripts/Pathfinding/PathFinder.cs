using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Behaviour searching the shortest path given a grid of obstacles (collider scanner)
/// TODO make it singleton
/// </summary>
public class PathFinder : MonoBehaviour
{
    private const float DefaultWeight = 1f;

    [SerializeField]
    [Tooltip("Optional, can be set at runtime")]
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
        [Range(1f, 5f)]
        public float Weight = 3f;
    }

    [Header("Debug")]
    [SerializeField]
    private bool _showDebug;
    [SerializeField]
    private Color _pathColor;

    private List<Vector2> _currentPath;
    private WorldGrid<ScanInfo> CollidersGrid => _colliderScanner.ScanResult;
    private LayerMask StaticLayer => _colliderScanner.GetStaticLayer();
    private Collider2D[] _collidersToIgnore;

    /// <summary>
    /// Finds the shortest path from start to final.
    /// Return a list of position, starting by the starting position.
    /// Always return a path.
    /// If the path is actually the shortest from start to target, then true i also return
    /// Otherwise, we find the best possible path (for instance if the target is not reachable), and false is returned in the tupple.
    /// </summary>
    /// <returns>The shortest path</returns>
    /// <param name="start">Start.</param>
    /// <param name="final">Final.</param>
    public List<Vector2> FindShortestPath(Vector2 start, Vector2 final, out bool couldReachTarget, bool onlyStaticLayer, Collider2D[] collidersToIgnore = null)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        Debug.Log("Search path from " + start + " to " + final);
        _collidersToIgnore = collidersToIgnore;
        var finalNode = new Node(CollidersGrid.GetGridIndex(final), null);

        // Preparation of the algorithm
        var startNode = new Node(CollidersGrid.GetGridIndex(start), null);
        var closeList = new List<Vector2Int>();
        var openList = new List<Node>();
        AddNodeInOpenList(openList, startNode);

        // Initialisation to keep track of the best candidate node
        couldReachTarget = false;
        startNode.Heuristic = float.MaxValue;
        var bestCandidate = startNode;

        int count = 0;
        //Debug.Log("Search path from node: " + startNode + " to " + finalNode);
        while (openList.Count > 0)
        {
            // Pick up the node with the lowest heuristic and remove it from the open list
            var node = openList[0];
            openList.Remove(node);
            // we reached the final position: return the path and stop algorithm
            if (node.Position == finalNode.Position)
            {
                couldReachTarget = true;
                bestCandidate = node;
                break;
            }
            // We check all the neighbours of the given node
            foreach (var neighbour in GetReachableNeighbours(node, _checkDiagonalNeighbours, onlyStaticLayer))
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
                if (neighbour.Heuristic < bestCandidate.Heuristic)
                    bestCandidate = neighbour;
            }
            // We add the node to the close if he doesnt exist already
            if (!closeList.Contains(node.Position))
                closeList.Add(node.Position);
            count++;
        }
        // couldnt find any path, we return the best possible path (from the best candidate node)
        watch.Stop();
        Debug.Log(string.Format("Found path in {0} iterations. Reached target: {1}", count, couldReachTarget));
        Debug.Log("Path finding computation time: " + (watch.ElapsedMilliseconds / 1000f));
        _currentPath = GetWorldCoordinatePath(bestCandidate);
        return _currentPath;
    }


    // Add a node in the open list at the good index (open list is sorted by ascendant heuristic)
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


    public void SetColliderScanner(ColliderScanner colliderScanner)
    {
        _colliderScanner = colliderScanner;
    }


    #region Utils
    // Get the neighbours of the given node
    private List<Node> GetReachableNeighbours(Node node, bool checkDiagonal, bool onlyStaticLayer)
    {
        var neighbours = new List<Node>();
        var center = new Vector2Int(node.Position.x, node.Position.y);
        var positions = onlyStaticLayer ?
                  _colliderScanner.GetReachablePositionFromSpecificRange(center, 1, StaticLayer, _collidersToIgnore)
                : _colliderScanner.GetReachablePositionFromSpecificRange(center, 1, _collidersToIgnore);
        foreach (var position in positions)
        {
            if (!checkDiagonal && (position.x != center.x && position.y != center.y))
                continue;
            neighbours.Add(new Node(position, node));
        }
        return neighbours;
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
    #endregion

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
        var scanInfo = CollidersGrid[node.Position.x, node.Position.y];
        var staticCollider = scanInfo.Get(StaticLayer);
        if (staticCollider != null && staticCollider.isTrigger)
        {
            // If the collider found has a tag defined in the weighted element list
            var weightedElement = _weightedElements.FirstOrDefault((x) => x.ObjectTag.Equals(staticCollider.tag));
            if (weightedElement != null)
                return weightedElement.Weight;
        }
        return DefaultWeight;
    }


    #endregion


    #region Debug
    private void OnDrawGizmos()
    {
        // area
        if (!_showDebug)
            return;
        // Draw Path
        Gizmos.color = _pathColor;
        if (_currentPath != null)
        {
            foreach (var point in _currentPath)
                Gizmos.DrawCube(point, Vector3.one * 0.5f);
        }
    }
    #endregion
}
