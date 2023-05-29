using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
    [SerializeField, Min(1)] int width;
    [SerializeField, Min(1)] int height;
    [SerializeField] int seedRandom;
    [SerializeField, Range(0f, 1f)] float spawnProbability = 0.1f;
    [SerializeField] private Cell floorPrefab;
    [SerializeField] private Stone lightStonePrefab;
    [SerializeField] private Stone darkStonePrefab;
    
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

        if (Random.value < spawnProbability)
        {
            var stoneLightLevel = Random.value > 0.5 ? LIGHT_LEVEL.LIGHT : LIGHT_LEVEL.DARK;
            newCell.SetStone(stoneLightLevel == LIGHT_LEVEL.LIGHT ? lightStonePrefab : darkStonePrefab);
        }
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

    public (int, int) GetCellCoordsFromPoint(Vector3 point)
    {
        return (Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.z));
    }

    public bool IsValidCell(int x, int y)
    {
        return cellReferences.ContainsKey((x, y));
    }

    public bool CanPlacePattern((int, int) center, GridTogglePattern selectedPattern)
    {
        return ProcessPattern(center, selectedPattern, (x, y) => IsValidCell(x, y), null);
    }

    public void ToggleCells((int, int) center, GridTogglePattern selectedPattern)
    {
        ProcessPattern(center, selectedPattern, (x, y) => IsValidCell(x, y), (x, y) =>
        {
            cellReferences[(x, y)].ToggleCell();
        });
    }

    public void HighlightCells((int, int) center, GridTogglePattern selectedPattern, bool validStatus)
    {
        ClearHighlightedCells();
        ProcessPattern(center, selectedPattern, (x, y) => IsValidCell(x, y), (x, y) =>
        {
            cellReferences[(x, y)].SetHighlight(validStatus);
            if (!highlightedCells.ContainsKey((x, y)))
                highlightedCells.Add((x, y), cellReferences[(x, y)]);
        });
    }

    public void ClearHighlightedCells()
    {
        if (highlightedCells != null)
        {
            foreach (var cell in highlightedCells)
            {
                cell.Value.ClearHighlight();
            }

            highlightedCells.Clear();
        }
    }


    private bool ProcessPattern((int, int) center, GridTogglePattern selectedPattern, Func<int, int, bool> condition, Action<int, int> action)
    {
        var patternSize = selectedPattern.patternSide.Length;
        var halfSizeVertical = selectedPattern.patternTop.Length > 0 ? 1 :
            0 + selectedPattern.patternSide.Length > 0 ? 1 :
            0 + selectedPattern.patternBottom.Length > 0 ? 1 : 0;
        var startX = center.Item1 - patternSize / 2;
        var startY = center.Item2 - halfSizeVertical;
        
        for (int x = 0; x < selectedPattern.patternTop.Length; x++)
        {
            if (!condition(startX + x, startY + 2))
            {
                return false;
            }
        }
        
        for (int x = 0; x < selectedPattern.patternSide.Length; x++)
        {
            if (!condition(startX + x, startY +1 ))
            {
                return false;
            }
        }
        
        for (int x = 0; x < selectedPattern.patternBottom.Length; x++)
        {
            if (!condition(startX + x, startY))
            {
                return false;
            }
        }

        if (action == null) return true;
        
        for (int x = 0; x < selectedPattern.patternTop.Length; x++)
        {
            action(startX + x, startY + 2);
        }
        
        for (int x = 0; x < selectedPattern.patternSide.Length; x++)
        {
            action(startX + x, startY +1 );
        }
        
        for (int x = 0; x < selectedPattern.patternBottom.Length; x++)
        {
            action(startX + x, startY );
        }

        return true;
    }
}
