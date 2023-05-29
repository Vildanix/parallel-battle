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

    public bool IsInRange(int testValue, int start, int end)
    {
        if (start > end)
            return testValue >= end && testValue <= start;
        return testValue >= start && testValue <= end;
    }

    public List<Cell> GetPathBetweenCells(Cell startCell, Cell targetCell)
    {
        var path = new List<Cell>();
        var currentPos = GetCellCoordsFromPoint(startCell.transform.position);
        var endPos = GetCellCoordsFromPoint(targetCell.transform.position);

        //path.Add(startCell); // TODO: replace with current player position to prevent rendering path from last cell during movement

        int totalDistance = Mathf.Max(Mathf.Abs(endPos.Item1 - currentPos.Item1), Mathf.Abs(endPos.Item2 - currentPos.Item2));
        for (int step = 0; step < totalDistance; step++ )
        {
            var stepDirection = GetOptimalStepDirection(currentPos, endPos);
            currentPos.Item1 += stepDirection.Item1;
            currentPos.Item2 += stepDirection.Item2;
            if (cellReferences.ContainsKey(currentPos))
            {
                path.Add(cellReferences[currentPos]);
            }
        }
        return path;
    }

    private (int, int) GetOptimalStepDirection((int, int) currentPos, (int, int) endPos)
    {
        var horizontalDiff = Mathf.Abs(currentPos.Item1 - endPos.Item1);
        var verticalDiff = Mathf.Abs(currentPos.Item2 - endPos.Item2);
        if (horizontalDiff > verticalDiff)
        {
            return (Mathf.RoundToInt(Mathf.Sign(endPos.Item1 - currentPos.Item1)), 0);
        }
        if (horizontalDiff < verticalDiff)
        {
            return (0, Mathf.RoundToInt(Mathf.Sign(endPos.Item2 - currentPos.Item2)));
        }

        return (Mathf.RoundToInt(Mathf.Sign(endPos.Item1 - currentPos.Item1)), Mathf.RoundToInt(Mathf.Sign(endPos.Item2 - currentPos.Item2))); 
    }
}
