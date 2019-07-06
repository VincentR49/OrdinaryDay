using System.Collections.Generic;
using UnityEngine;

// Generic class to store some information contained in the world map on grid
public class WorldGrid<T>
{
    private T[,] _grid;
    public int NRow => _grid.GetLength(0);
    public int NColumn => _grid.GetLength(1);

    // link with world coordinate
    private Rect _rect;
    private float _cellSize;

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
        var nRow = (int) Mathf.Ceil(rect.height / cellSize);
        var nCol = (int) Mathf.Ceil(rect.width / cellSize);
        _grid = new T[nRow, nCol];
        Debug.Log("Create World Grid");
        Debug.Log("N Elements: " + NRow * NColumn);
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
        for (int row = 0; row < NRow; row++)
        {
            for (int column = 0; column < NColumn; column++)
                list.Add(GetWordCoordinate(row, column));
        }
        return list;
    }


    public Vector2 GetWordCoordinate(int row, int column)
    {
        var x = _rect.x + column * _cellSize + _cellSize / 2; // middle
        var y = _rect.y + row * _cellSize + _cellSize / 2; // middle
        return new Vector2(x, y);
    }


    public Vector2Int GetGridIndex(Vector2 worldPosition)
    {
        var row = (int) Mathf.Ceil ((worldPosition.y - _rect.yMin) / _cellSize) - 1;
        var column = (int) Mathf.Ceil ((worldPosition.x - _rect.xMin) / _cellSize) - 1;
        return new Vector2Int(row, column);
    }


    public Vector2 GetWordCoordinate(Vector2Int gridIndex) => GetWordCoordinate(gridIndex.x, gridIndex.y);
}
