using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using UnityEngine;

public class Grid<TGridObject>
{

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
        public Color color;
    }

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;

    private TGridObject[,] gridArray;
    private TextMesh[,] debugTextArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int w = 0; w < width; ++w)
        {
            for (int h = 0; h < height; ++h)
            {
                gridArray[w, h] = createGridObject(this, w, h);
            }
        }

        for (int w = 0; w < width; ++w)
        {
            for (int h = 0; h < height; ++h)
            {
                debugTextArray[w, h] = CreateWorldText(gridArray[w, h]?.ToString(), null, GetWorldPosition(w, h) + new Vector3(cellSize, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(w, h), GetWorldPosition(w, h + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(w, h), GetWorldPosition(w + 1, h), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0),  GetWorldPosition(width, height), Color.white, 100f);

        OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
        {
            debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y].ToString();
            debugTextArray[eventArgs.x, eventArgs.y].color = eventArgs.color;
        };

    }

    internal int GetHeight()
    {
        return height;
    }

    internal int GetWidth()
    {
        return width;
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, Color.white, value);
    }

    public void SetGridObject(int x, int y, Color color, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y]?.ToString();
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y, color = color });
        }
    }

    public void TriggerGridObjectChanged(int x, int y, Color color)
    {
        if(OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y, color = color });
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition.x - originPosition.x)/ cellSize);
        y = Mathf.FloorToInt((worldPosition.y - originPosition.y) / cellSize);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    private static TextMesh CreateWorldText(string Text, Transform parent, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment = TextAlignment.Center)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;

        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = Text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;

        return textMesh;
    }

}
