using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Grid : MonoBehaviour
{
    [SerializeField, Min(1)] int width;
    [SerializeField, Min(1)] int height;
    [SerializeField] int seedRandom;
    [SerializeField, Range(0f, 1f)] float spawnProbability = 0.1f;
    [SerializeField] private Cell floorPrefab;
    
    Dictionary<(int, int), Cell> cellReferences = new();
    Dictionary<(int, int), Cell> highlightedCells = new();

    private void Awake()
    {
        Initialize();
        Random.InitState(seedRandom);
    }

    public void Initialize()
    {
        for (int x = 0; x < width ; x++)
        {
            for (int y = 0; y < height ; y++)
            {
                CreateCell(x, y, floorPrefab);
            }
        }
    }

    private void CreateCell(int x, int y, Cell prefab)
    {
        var newCell = Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity, transform);
        newCell.name = $"Cell {x}, {y}";
        cellReferences.Add((x, y), newCell);

    }

    public Cell GetCellAtPosition(Vector3 position)
    {
        var posX = Mathf.RoundToInt(position.x);
        var posY = Mathf.RoundToInt(position.z);
        if (cellReferences.ContainsKey((posX, posY))) {
            return cellReferences[(posX, posY)];
        }

        return null;
    }

    public void SetCenterPosition(Vector3 position)
    {
        var coords = GetCellCoordsFromPoint(position);
    }

    private (int, int) GetCellCoordsFromPoint(Vector3 point)
    {
        return (Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.z));
    }

    public bool IsValidCell(int x, int y)
    {
        return cellReferences.ContainsKey((posX, posY));
    }

    public bool CanPlacePattern((int, int) center, int patternSize)
    {
        return ProcessPattern(center, patternSize, (x, y) => !grid.IsValidCell(x, y), null);
    }

    public void ToggleCells((int, int) center, int patternSize)
    {
        ProcessPattern(center, patternSize, nu(x, y) => !grid.IsValidCell(x, y), (x, y) =>
        {
            cellReferences[(posX, posY)].ToggleCell();
        });
    }

    public void HighlightCells((int, int) center, int patternSize, bool validStatus)
    {
        ProcessPattern(center, patternSize, nu(x, y) => !grid.IsValidCell(x, y), (x, y) =>
        {
            cellReferences[(posX, posY)].SetHighlight();
            highlightedCells.Add((posX, posY), cellReferences[(posX, posY)]);
        });
    }

    public void ClearHighlightedCells()
    {
        if (highlightedCells != null)
        {
            foreach (var cell in highlightedCells)
            {
                if (cell != null)
                {
                    cell.ClearHighlight();
                }
            }

            highlightedCells = null;
        }
    }


    private bool ProcessPattern((int, int) center, int patternSize, Func<int, int, bool> condition, Action<int, int> action)
    {
        var halfSize = patternSize / 2;
        var startX = center.Item1 - halfSize;
        var startY = center.Item2 - halfSize;

        for (int x = startX; x < startX + patternSize; x++)
        {
            for (int y = startY; y < startY + patternSize; y++)
            {
                if (!condition(x, y))
                {
                    return false;
                }
            }
        }

        for (int x = startX; x < startX + patternSize; x++)
        {
            for (int y = startY; y < startY + patternSize; y++)
            {
                action(x, y);
            }
        }

        return true;
    }
}
