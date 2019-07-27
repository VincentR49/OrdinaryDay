using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generic class to store some information contained in the world map on grid
public class WorldGrid<T> : IEnumerator, IEnumerable
{
    private T[,] _grid;
    public int Nx => _grid.GetLength(0);
    public int Ny => _grid.GetLength(1);

    public object Current => throw new System.NotImplementedException();

    // link with world coordinate
    private Rect _rect;
    private float _cellSize;
    private Vector2Int _position;


    // Iterator
    public T this[int x ,int y]
    {
        get { return _grid[x,y]; }
        set { _grid[x,y] = value; }
    }

    public WorldGrid(Rect rect, float cellSize)
    {
        _rect = rect;
        _cellSize = cellSize;
        // grid initialization
        var nX = (int) Mathf.Ceil(rect.width / cellSize);
        var nY = (int) Mathf.Ceil(rect.height / cellSize);
        _grid = new T[nX, nY];
        Debug.Log("Create World Grid containing: " + Nx * Ny + " elements.");
    }


    public void Set(Vector2 worldPosition, T value)
    {
        var index = GetGridIndex(worldPosition);
        //Debug.Log(string.Format("Set at {0} from world position {1}", string.Join(", ", index), worldPosition.ToString()));
        _grid[index.x, index.y] = value;
    }


    public T Get(Vector2 worldPosition)
    {
        var gridIndex = GetGridIndex(worldPosition);
        return _grid[gridIndex.x, gridIndex.y];
    }


    public List<Vector2> GetWorldCooordinateList()
    {
        var list = new List<Vector2>();
        for (int x = 0; x < Nx; x++)
        {
            for (int y = 0; y < Ny; y++)
                list.Add(GetWordCoordinate(x, y));
        }
        return list;
    }


    public Vector2 GetWordCoordinate(int x, int y)
    {
        var wx = _rect.x + x * _cellSize + _cellSize / 2; // middle
        var wy = _rect.y + y * _cellSize + _cellSize / 2; // middle
        return new Vector2(wx, wy);
    }


    public Vector2Int GetGridIndex(Vector2 worldPosition)
    {
        var x = (int) Mathf.Ceil ((worldPosition.x - _rect.xMin) / _cellSize) - 1;
        var y = (int) Mathf.Ceil ((worldPosition.y - _rect.yMin) / _cellSize) - 1;
        return new Vector2Int(x, y);
    }


    public Vector2 GetWordCoordinate(Vector2Int gridIndex) => GetWordCoordinate(gridIndex.x, gridIndex.y);


    public IEnumerator GetEnumerator()
    {
        return (IEnumerator) this;
    }

    public bool MoveNext()
    {
        if (_position.x < Nx - 1)
            _position.x++;
        else
        {
            _position.y++;
            _position.x = 0;
        }
        return _position.y < Ny;
    }

    public void Reset()
    {
        _position = Vector2Int.zero;
    }
}
