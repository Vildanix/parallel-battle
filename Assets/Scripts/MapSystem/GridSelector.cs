using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GridSelector : MonoBehaviour
{
    [SerializeField] LayerMask mask;
    [SerializeField] Grid grid;

    private void Update()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition;);
        if(Physics.Raycast(ray, out var hit, Mathf.Infinity, mask))
        {
            var cell = grid.GetCellAtPosition(hit.point);

            if (cell != null)
            {
                var coords = grid.GetCellCoordsFromPoint(cell.transform.position);

                if (grid.CanPlacePattern(coords, patternSize))
                {
                    // Highlight cells valid placement
                    
                    if (Input.OnMouseButtonDown(0)) {
                        ToggleCells(coords, patternSize);
                    }
                } else {
                    // Highlight cells invalid placement
                }
            }
        }
    }
}
