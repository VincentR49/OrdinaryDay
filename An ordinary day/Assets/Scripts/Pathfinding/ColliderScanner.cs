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
    [Tooltip("Put 0 for no automatic refresh")]
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
    public WorldGrid<ScanInfo> ScanResult { get; private set; }
    private List<Vector2> _pointsToScan; // points to scan during for the dynamic layer

    #region Init
    private void InitScanGrid()
    {
        ScanResult = new WorldGrid<ScanInfo>(_scanArea, _spacialResolution);
        for (int x = 0; x < ScanResult.Nx; x++)
        {
            for (int y = 0; y < ScanResult.Ny; y++)
                ScanResult[x, y] = new ScanInfo();
        }
        _pointsToScan = ScanResult.GetWorldCooordinateList();
    }


    private void Awake()
    {
        InitScanGrid();
        ScanStaticLayers();
    }
    #endregion

    #region Get / Set

    public LayerMask GetStaticLayer() => _staticLayerToScan;
    public LayerMask GetDynamicLayer() => _dynamicLayerToScan;

    #endregion

    private void Update()
    {
        if (_refreshRateSec > 0f && (Time.time >= _lastRefreshTime + _refreshRateSec))
        {
            ScanDynamicLayers();
        }
    }


    #region Scanning

    /// <summary>
    /// Scan layers on all the scan area (only dynamic layers)
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
        var points = _pointsToScan.FindAll((x) => Utils.Distance(center, x) <= radius);
        if (points != null)
        {
           // Debug.Log("Perform local scan on " + points.Count + " points.");
            Scan(_dynamicLayerToScan, points);
        }
    }


    private void ScanStaticLayers()
    {
        Debug.Log("[ColliderScanner] Scan Static Layer");
        Scan(_staticLayerToScan);
        // Remove all the static points that already have colliders
        var nBefore = _pointsToScan.Count;
        // we removed all the points containing solid colliders on the static layer, for optiisation purpose
        _pointsToScan.RemoveAll ((point) =>
            ScanResult.Get(point).HasSolidCollider(_staticLayerToScan)
        ); 
        var nAfter = _pointsToScan.Count;
        Debug.Log("Scan optimization. Removed " + (nBefore - nAfter) + " points to scan.");
    }


    private void ScanDynamicLayers()
    {
        Debug.Log("[ColliderScanner] ScanDynamicLayers");
        _lastRefreshTime = Time.time;
        Scan(_dynamicLayerToScan);
    }


    /// <summary>
    /// Scan a layer in the given world points
    /// </summary>
    private void Scan(LayerMask layer, List<Vector2> points)
    {
        foreach (var point in points)
        {
            var scanInfo = ScanResult.Get(point);
            scanInfo.Set(layer, ScanPosition(point, layer));
        }
    }

    /// <summary>
    /// Scan a complete layer on all the scan area
    /// </summary>
    /// <param name="layer"></param>
    private void Scan(LayerMask layer)
    {
        Scan(layer, _pointsToScan);
    }

    #endregion


    #region Utils
    private Collider2D ScanPosition(Vector2 point, LayerMask layer)
    {
        return Physics2D.OverlapCircle(point, _spacialResolution / ScanRadiusDivider, layer);
    }

    /// <summary>
    /// Search all the accessible points located at the given range from the center point in all layers mask.
    /// Those points are located on the border of a square area.
    /// <returns></returns>
    public List<Vector2Int> GetReachablePositionFromSpecificRange(Vector2Int center, int range, Collider2D[] collidersToIgnore = null)
        => GetReachablePositionFromSpecificRange(center, range, new LayerMask[] { _staticLayerToScan, _dynamicLayerToScan }, collidersToIgnore);


    /// <summary>
    /// Search all the accessible points located at the given range from the center point in a specific layer.
    /// Those points are located on the border of a square area.
    /// <returns></returns>
    public List<Vector2Int> GetReachablePositionFromSpecificRange(Vector2Int center, int range, LayerMask layer, Collider2D[] collidersToIgnore = null)
        => GetReachablePositionFromSpecificRange(center, range, new LayerMask[] { layer }, collidersToIgnore);


    /// <summary>
    /// Search all the accessible points located at the given range from the center point in the defined layers.
    /// Those points are located on the border of a square area.
    /// <returns></returns>
    public List<Vector2Int> GetReachablePositionFromSpecificRange(Vector2Int center, int range, LayerMask[] layers, Collider2D[] collidersToIgnore = null)
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
                if (Mathf.Abs(center.x - x) != range && Mathf.Abs(center.y - y) != range)
                    continue;
                var hasCollider = false;
                // Check collision on all the layer mask defined
                foreach (var layer in layers)
                    hasCollider = hasCollider || HasCollider(x, y, layer, collidersToIgnore);
                if (!hasCollider)
                    positions.Add(new Vector2Int(x, y));
            }
        }
        return positions;
    }

    /// <summary>
    /// Return true if the given position has a collider in the given LayerMask
    /// </summary>
    private bool HasCollider(int x, int y, LayerMask layerMask, Collider2D[] collidersToIgnore = null)
    {
        var collision = ScanResult[x, y].Get(layerMask);
        if (collision == null
             || collision.isTrigger
             || (collidersToIgnore != null && collidersToIgnore.Contains(collision)))
        {
            return false;
        }
        return true;
    }

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
                    var staticCollider = ScanResult[x, y].Get(_staticLayerToScan);
                    var dynamicCollider = ScanResult[x, y].Get(_dynamicLayerToScan);
                    if (dynamicCollider)
                    {
                        if (dynamicCollider.isTrigger)
                            triggerPoints.Add(ScanResult.GetWordCoordinate(x, y));
                        else
                            colliderPoints.Add(ScanResult.GetWordCoordinate(x, y));
                        continue;
                    }
                    if (staticCollider)
                    {
                        if (staticCollider.isTrigger)
                            triggerPoints.Add(ScanResult.GetWordCoordinate(x, y));
                        else
                            colliderPoints.Add(ScanResult.GetWordCoordinate(x, y));
                    }
                }
            }
        }
    }
    #endregion
}
