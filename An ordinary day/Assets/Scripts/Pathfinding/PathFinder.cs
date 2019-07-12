using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Behaviour searching the shortest path given a grid of obstacles (collider scanner)
/// </summary>
public class PathFinder : MonoBehaviour
{
    [SerializeField]
    private ColliderScanner _colliderScanner; // obstacle grid

    [Header("Parameters")]
    [SerializeField]
    [Tooltip("If true, check also the neighbours in diagonal direction")]
    private bool _checkDiagonalNeighbours = true;

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


    /// <summary>
    /// Finds the shortest path from start to final.
    /// Return a list of position, starting by the starting position.
    /// Return null if any path could be found or if the target is unreachable.
    /// </summary>
    /// <returns>The shortest path.</returns>
    /// <param name="start">Start.</param>
    /// <param name="final">Final.</param>
    public List<Vector2> FindShortestPath(Vector2 start, Vector2 final)
    {
        Debug.Log("Search path from " + start + " to " + final);
        var finalNode = new Node(CollidersGrid.GetGridIndex(final), null);

        if (CollidersGrid[finalNode.Position.x, finalNode.Position.y])
        {
            Debug.LogError("Target is unreachable.");
            return null;
        }

        // Preparation of the algorithm
        var startNode = new Node(CollidersGrid.GetGridIndex(start), null);
        var closedList = new List<Node>();
        var openedList = new List<Node>();
        openedList.Add(startNode);

        int count = 0;
        Debug.Log("Search path from node) " + startNode + " to " + finalNode);
        while (openedList.Count > 0)
        {
            // Pick up the node with the lowest heuristic and remove it from the open list
            var node = openedList[0]; 
            openedList.Remove(node);
            Debug.Log("Iteration " + count + ". Node: " + node);
            // we reached the final position: return the path and stop algorithm
            if (node.Position == finalNode.Position)
            {
                Debug.Log("Path found !");
                return GetWorldCoordinatePath(node);
            }
            // We check all the neighbours of the given node
            foreach (var neighbour in GetReachableNeighbours(node))
            {
                // already in closed list, dont take into account
                if (closedList.ContainsNode(neighbour))
                    continue;
                // pass if already in opened list and its cost is superior
                var neighBourInOpenList = openedList.Get(neighbour);
                if (neighBourInOpenList != null)
                {
                    // less interesting node, so we just skip this one
                    if (neighBourInOpenList.Cost <= neighbour.Cost) 
                        continue;
                    else // we remove the previous version of the node from the opened list
                        openedList.Remove(neighBourInOpenList);
                } 
                // otherwise, we add to the opened list
                neighbour.Cost = node.Cost + neighbour.GetDistanceFrom(node); // compute cost
                neighbour.Heuristic = ComputeHeuristic(neighbour, finalNode);
                openedList.Add(neighbour);
            }
            // We add the node to the close if he doesnt exist already
            if (!closedList.Contains(node))
                closedList.Add(node);
            // Sort by heuristic value
            openedList.Sort();
            Debug.Log("New closed List: " + string.Join(", ", closedList));
            Debug.Log("New open List: " + string.Join(", ", openedList));
            count++;
        }
        // couldnt find any path
        Debug.LogError("Couldnt find any path from: " + start + " to " + final);
        return null;
    }


    // Get the neighbours of the given node
    private List<Node> GetReachableNeighbours(Node node)
    {
        var neighbours = new List<Node>();
        var nodeX = node.Position.x; // sugar
        var nodeY = node.Position.y; // sugar
        for (int x = Mathf.Max(nodeX - 1, 0);
                 x <= Mathf.Min(nodeX+ 1,CollidersGrid.Nx-1); x++)
        {
            for (int y = Mathf.Max(nodeY - 1, 0);
                     y <= Mathf.Min(nodeY + 1, CollidersGrid.Ny-1); y++)
            {
                // not reachable or same node as origin
                if ((nodeX == x && nodeY == y) || CollidersGrid[x,y])
                    continue;
                if (!_checkDiagonalNeighbours && (x != nodeX && y != nodeY))
                    continue;
                neighbours.Add(new Node(new Vector2Int(x, y), node));
            }
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


    private static float ComputeHeuristic(Node node, Node target)
        => node.Cost + node.GetDistanceFrom(target);


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
