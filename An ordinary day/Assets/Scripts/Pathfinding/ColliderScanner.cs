using System.Collections.Generic;
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
    private float _refreshRateSec = 1f;
    [SerializeField]
    private Rect _scanArea;

    [Header("Layers")]
    [SerializeField]
    private LayerMask[] _dynamicLayersToScan;
    [SerializeField]
    private LayerMask[] _staticLayersToScan;

    [Header("Debug")]
    [SerializeField]
    private bool _showDebug = true;
    [SerializeField]
    private Texture2D _areaTexture;
    [SerializeField]
    private Color _nOkColor;

    private float _lastRefreshTime;
    public WorldGrid<bool> ScanResult { get; private set; }
    private List<Vector2> _pointsToScan; // store in order to optimize computations


    #region Init
    private void InitScanGrid()
    {
        ScanResult = new WorldGrid<bool>(_scanArea, _spacialResolution);
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
    private void ScanStaticLayers()
    {
        Debug.Log("[ColliderScanner] Scan Static Layer");
        Scan(_staticLayersToScan);
        // Remove all the static points that already have colliders
        var n = _pointsToScan.Count;
        _pointsToScan.RemoveAll ((point) => ScanResult.Get(point));
        var nAfterOpti = _pointsToScan.Count;
        Debug.Log("Scan optimization. Removed " + (n - nAfterOpti) + " points to scan.");
    }


    private void ScanDynamicLayers()
    {
        Debug.Log("[ColliderScanner] ScanDynamicLayers");
        _lastRefreshTime = Time.time;
        Scan(_dynamicLayersToScan);
    }


    private void Scan(LayerMask[] layers)
    {
        if (layers == null || layers.Length == 0)
            return;
        foreach (var layer in layers)
            Scan(layer);
    }


    private void Scan(LayerMask layer)
    {
        foreach (var point in _pointsToScan)
            ScanResult.Set(point, HasCollider(point, layer));
    }

    #endregion


    #region Utils
    private bool HasCollider(Vector2 point, LayerMask layer)
    {
        return Physics2D.OverlapCircle(point, _spacialResolution / ScanRadiusDivider, layer) != null;
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
        var nOKPoints = GetAllNOkPoints();
        Gizmos.color = _nOkColor;
        foreach (var point in nOKPoints)
            Gizmos.DrawCube(point, Vector3.one * 0.5f);
                     
    }


    private List<Vector2> GetAllNOkPoints()
    {
        var nOKPoints = new List<Vector2>();
        if (ScanResult != null)
        {
            for (int x = 0; x < ScanResult.Nx; x++)
            {
                for (int y = 0; y < ScanResult.Ny; y++)
                {
                    if (ScanResult[x, y])
                        nOKPoints.Add(ScanResult.GetWordCoordinate(x, y));
                }
            }
        }
        return nOKPoints;
    }
    #endregion
}
