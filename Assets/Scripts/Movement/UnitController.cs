using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private Locator locator;

    private MapData mapData;

    [SerializeField] private float movementSpeed;
    private void Awake()
    {
        // initializaze dependencies with Locator.Get
        Locator.Get<MapData>((mapData) => this.mapData = mapData as MapData);
    }

    private void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        // Get input for movement up / left / right / down
        // for each input try to move this unit in that direction one cell and lock movement unitl new cell is reached
        // mapData have function to get cell: mapData.GetTile(x, y)
    }
}
