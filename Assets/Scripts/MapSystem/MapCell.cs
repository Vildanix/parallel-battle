using System;
using System.Collections.Generic;
using UnityEngine;

namespace MapSystem
{
    public class MapCell
    {
        [field: SerializeField] public GameObject Instance { get; protected set; }
        [field: SerializeField] public List<MapCell> Neighbors { get; protected set; }
        [field: SerializeField] public CellType Type { get; protected set; }

        // Constructor
        public MapCell(CellType type, GameObject prefab, Vector3 position, Transform parent)
        {
            Type = type;
            Neighbors = new List<MapCell>();
            Instance = GameObject.Instantiate(prefab, position, Quaternion.identity, parent);
        }
        
        public void AddNeighbor(MapCell neighbor)
        {
            if (!Neighbors.Contains(neighbor))
            {
                Neighbors.Add(neighbor);
                neighbor.Neighbors.Add(this);
            }
        }
        
        public void RemoveNeighbor(MapCell neighbor)
        {
            if (Neighbors.Contains(neighbor))
            {
                Neighbors.Remove(neighbor);
                neighbor.Neighbors.Remove(this);
            }
        }
    }

    public enum CellType
    {
        None,
        Wall,
        Floor,
        Obstacle
    }
}