using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GridSelector : MonoBehaviour
{
    [SerializeField] LayerMask mask;
    [SerializeField] Grid grid;
    [SerializeField] private GridTogglePattern selectedPattern;
    [SerializeField] private List<GridTogglePattern> patternList;

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, mask))
        {
            var coords = grid.GetCellCoordsFromPoint(hit.point);

            if (grid.CanPlacePattern(coords, selectedPattern))
            {
                grid.HighlightCells(coords, selectedPattern, true);

                if (Input.GetMouseButtonDown(0))
                {
                    grid.ToggleCells(coords, selectedPattern);
                    selectedPattern = patternList[Mathf.RoundToInt(Random.Range(0, patternList.Count))];
                }
            }
            else
            {
                grid.HighlightCells(coords, selectedPattern, false);
            }
        }
        else
        {
            grid.ClearHighlightedCells();
        }
    }
}
