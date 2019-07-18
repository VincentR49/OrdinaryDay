using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Scan the presence of colliders in the given area
/// </summary>
public class ColliderScanner : MonoBehaviour
{
    private const float ScanRadiusDivider = 3f;

    [Header("Scan parameters")]
    [SerializeField]
    [Tooltip("Unity distance unit")]
    private float _spacialResolution = 1f;
    [SerializeField]
    private float _refreshRateSec = 5f;
    [SerializeField]
    private Rect _scanArea;

    [Header("Layers")]
    [SerializeField]
    private LayerMask _dynamicLayerToScan;
    [SerializeField]
    private LayerMask _staticLayerToScan;

    [Header("Debug")]
    [SerializeField]
    private bool _showDebug = true;
    [SerializeField]
    private Texture2D _areaTexture;
    [SerializeField]
    private Color _collidersColor;
    [SerializeField]
    private Color _triggerColors;

    private float _lastRefreshTime;
    public WorldGrid<Collider2D> ScanResult { get; private set; }
    private List<Vector2> _pointsToScan; // store in order to optimize computations


    #region Init
    private void InitScanGrid()
    {
        ScanResult = new WorldGrid<Collider2D>(_scanArea, _spacialResolution);
        _pointsToScan = ScanResult.GetWorldCooordinateList();
    }


    private void Start()
    {
        InitScanGrid();
        ScanStaticLayers();
        ScanDynamicLayers();
        //Debug.Log("Collider at debug position: " + _scanResult.Get(new Vector2(-9.42f, 1.12f)));
    }
    #endregion


    private void Update()
    {
        if (Time.time >= _lastRefreshTime + _refreshRateSec)
        {
            ScanDynamicLayers();
        }
    }


    #region Scanning

    /// <summary>
    /// Scan dynamic layers on all the scan area
    /// </summary>
    public void Scan()
    {
        ScanDynamicLayers();
    }

    /// <summary>
    /// Scan dynamic layers on restricted are
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    public void Scan(Vector2 center, float radius)
    {
        // TODO
        Debug.LogError("TODO: Localized scan");
        ScanDynamicLayers();
    }


    private void ScanStaticLayers()
    {
        Debug.Log("[ColliderScanner] Scan Static Layer");
        Scan(_staticLayerToScan);
        // Remove all the static points that already have colliders
        var n = _pointsToScan.Count;
        // ze removed all the non trigger objects
        _pointsToScan.RemoveAll ((point) =>
        {
            var scanedCollider = ScanResult.Get(point);
            return scanedCollider != null; // && !scanedCollider.isTrigger;
        }); 
        var nAfterOpti = _pointsToScan.Count;
        Debug.Log("Scan optimization. Removed " + (n - nAfterOpti) + " points to scan.");
    }


    private void ScanDynamicLayers()
    {
        Debug.Log("[ColliderScanner] ScanDynamicLayers");
        _lastRefreshTime = Time.time;
        Scan(_dynamicLayerToScan);
    }

    private void Scan(LayerMask layer)
    {
        foreach (var point in _pointsToScan)
            ScanResult.Set(point, ScanPosition(point, layer));
    }

    #endregion


    #region Utils
    private Collider2D ScanPosition(Vector2 point, LayerMask layer)
    {
        return Physics2D.OverlapCircle(point, _spacialResolution / ScanRadiusDivider, layer);
    }


    /// <summary>
    /// Search all the accessible points located at the given range from the center point.
    /// Those points are located on the border of a square area.
    /// </summary>
    /// <param name="center"></param>
    /// <param name="range"></param>
    /// <param name="_collidersToIgnore"></param>
    /// <returns></returns>
    public List<Vector2Int> GetReachablePositionFromSpecificRange(Vector2Int center, int range, Collider2D[] _collidersToIgnore = null)
    {
        var positions = new List<Vector2Int>();

        var minY = Mathf.Max(center.y - range, 0);
        var maxY = Mathf.Min(center.y + range, ScanResult.Ny - 1);

        var minX = Mathf.Max(center.x - range, 0);
        var maxX = Mathf.Min(center.x + range, ScanResult.Nx - 1);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if ((   Mathf.Abs(center.x - x) != range
                    &&  Mathf.Abs(center.y - y) != range) 
                    ||  HasCollider(x, y, _collidersToIgnore))
                    continue;
                positions.Add(new Vector2Int(x, y));
            }
        }
        return positions;
    }

    /// <summary>
    /// Return true if the given position has a collider.
    /// </summary>
    private bool HasCollider(int x, int y, Collider2D[] collidersToIgnore = null)
    {
        if (ScanResult[x, y] == null // no collider
                                        // if the collider has to be ignored, return false                            
            || ScanResult[x, y].isTrigger
            || (collidersToIgnore != null && collidersToIgnore.Contains(ScanResult[x, y]))
             )
            return false;
        return true;
    }


    public bool HasCollider(Vector2Int position, Collider2D[] collidersToIgnore = null)
        => HasCollider(position.x, position.y, collidersToIgnore);

    #endregion


    #region Debug
    private void OnDrawGizmos()
    {
        // area
        if (!_showDebug)
            return;
        Gizmos.DrawGUITexture(_scanArea, _areaTexture);
        // Draw nOk Areas
        GetCollidersInGrid(out List<Vector2> colliderPoints, out List<Vector2> triggerPoints);
        Gizmos.color = _collidersColor;
        foreach (var point in colliderPoints)
            Gizmos.DrawCube(point, Vector3.one * _spacialResolution / 2f);
        Gizmos.color = _triggerColors;
        foreach (var point in triggerPoints)
            Gizmos.DrawCube(point, Vector3.one * _spacialResolution / 2f);
    }


    private void GetCollidersInGrid(out List<Vector2> colliderPoints, out List<Vector2> triggerPoints)
    {
        colliderPoints = new List<Vector2>();
        triggerPoints = new List<Vector2>();
        if (ScanResult != null)
        {
            for (int x = 0; x < ScanResult.Nx; x++)
            {
                for (int y = 0; y < ScanResult.Ny; y++)
                {
                    if (ScanResult[x, y])
                    {
                        if (ScanResult[x,y].isTrigger)
                        {
                            triggerPoints.Add(ScanResult.GetWordCoordinate(x, y));
                        }
                        else
                        {
                            colliderPoints.Add(ScanResult.GetWordCoordinate(x, y));
                        }
                    }
                        
                }
            }
        }
    }
    #endregion
}
