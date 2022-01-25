using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Grid class for X and Z coordinates
 */
public class GridXYZ<TGridObject>
{
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
        public int z;
    }


    private int width;
    private int height;
    private int depth;
    private float cellSize;
    private GameObject parent;
    private Vector3 originPosition;
    private TGridObject[,,] gridArray;

    public GridXYZ(int width, int height, int depth, float cellSize, GameObject parent, Vector3 originPosition, Func<GridXYZ<TGridObject>, int, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.depth = depth;
        this.cellSize = cellSize;
        this.parent = parent;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height, depth];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                for(int z = 0; z < gridArray.GetLength(2); z++)
                {
                    gridArray[x, y, z] = createGridObject(this, x, y, z);
                }
            }
        }

        bool showDebug = true;
        if (showDebug)
        {
            TextMesh[,,] debugTextArray = new TextMesh[width, height, depth];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    for (int z = 0; z < gridArray.GetLength(2); z++)
                    {
                        debugTextArray[x, y, z] = TextUtils.CreateWorldText(gridArray[x, y, z]?.ToString(), parent.transform, GetWorldPosition(x, y, z) + new Vector3(cellSize, 0, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Left, 5000, new Vector3(cellSize, 0, cellSize));
                        //Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                        //Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
                    }
                }
            }
            //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            //Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.y, eventArgs.z].text = gridArray[eventArgs.x, eventArgs.y, eventArgs.z]?.ToString();
            };

        }
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public Vector3 GetWorldPosition(int x, int y, int z)
    {
        Vector3 position = new Vector3(x, y, z) * cellSize + originPosition;
        return position;
    }

    public void GetXYZ(Vector3 worldPosition, out int x, out int y, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    }

    public void SetGridObject(int x, int y, int z, TGridObject value)
    {
        if (x >= 0 && y >= 0 && z >= 0 && x < width && y < height && z < depth)
        {
            gridArray[x, y, z] = value;
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y, z = z });
        }
    }

    public void TriggerGridObjectChanged(int x, int y, int z)
    {
        if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y, z = z });
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, y, z;
        GetXYZ(worldPosition, out x, out y, out z);
        SetGridObject(x, y, z, value);
    }

    public TGridObject GetGridObject(int x, int y, int z)
    {
        if (x >= 0 && y >= 0 && z >= 0 && x < width && y < height && z < depth)
        {
            return gridArray[x, y, z];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y, z;
        GetXYZ(worldPosition, out x, out y, out z);
        return GetGridObject(x, y, z);
    }
}

