using MapSystem;
using UnityEngine;

public class MapData : MonoBehaviour
{
    public int width; // Width of the map in tiles
    public int height; // Height of the map in tiles
    public float cellSpacing = 1.0f; // Distance between cells when instantiating
    public MapCell[,] tiles; // 2D array to store tile types
    public GameObject WallPrefab;
    public GameObject FloorPrefab;
    public GameObject ObstaclePrefab;

    public void InitializeMap()
    {
        tiles = new MapCell[width, height];

        // Build the grid of cells with a border made from walls
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    // Create a wall cell for the border
                    Vector3 position = new Vector3(x * cellSpacing, 0f, y * cellSpacing);
                    tiles[x, y] = new MapCell(CellType.Wall, WallPrefab, position, transform);
                }
            }
        }

        // Build the inside cells
        for (int x = 2; x < width - 1; x += 2)
        {
            for (int y = 2; y < height - 1; y += 2)
            {
                Vector3 position = new Vector3(x * cellSpacing, 0f, y * cellSpacing);

                // Add obstacles randomly
                if (Random.value < 0.3f)
                {
                    tiles[x, y] = new MapCell(CellType.Obstacle, ObstaclePrefab, position, transform);
                }
                else
                {
                    tiles[x, y] = new MapCell(CellType.Floor, FloorPrefab, position, transform);
                }
            }
        }
    }

    public void InitializeMirroredMap(MapData originalMap)
    {
        width = originalMap.width * 2;
        height = originalMap.height;
        tiles = new MapCell[width, height];

        // Copy the original map's cells
        for (int x = 0; x < originalMap.width; x++)
        {
            for (int y = 0; y < originalMap.height; y++)
            {
                MapCell originalCell = originalMap.tiles[x, y];
                CellType type = originalCell.Type;
                Vector3 position = new Vector3(x * cellSpacing, 0f, y * cellSpacing);

                tiles[x, y] = new MapCell(type, originalCell.Instance, position, transform);
            }
        }

        // Create mirrored cells
        for (int x = originalMap.width; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                MapCell mirroredCell = tiles[width - x - 1, y];
                Vector3 position = new Vector3(x * cellSpacing, 0f, y * cellSpacing);

                tiles[x, y] = new MapCell(mirroredCell.Type, mirroredCell.Instance, position, transform);
            }
        }
    }

    // Get the MapCell at a specific position in world space
    public MapCell GetMapCellByWorldPosition(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / cellSpacing);
        int y = Mathf.FloorToInt(position.z / cellSpacing);

        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return tiles[x, y];
        }

        return null;
    }

    // Get the MapCell at a specific position on the screen (mouse position)
    public MapCell GetMapCellByScreenPosition(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return GetMapCellByWorldPosition(hit.point);
        }

        return null;
    }

    // Get the neighboring MapCell based on the direction vector and current world position
    public MapCell GetNeighboringMapCell(Vector3 direction, Vector3 position)
    {
        Vector3 newPosition = position + direction.normalized * cellSpacing;
        return GetMapCellByWorldPosition(newPosition);
    }
}