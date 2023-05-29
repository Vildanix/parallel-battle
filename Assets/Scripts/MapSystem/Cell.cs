using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class Cell : MonoBehaviour
{
    [Header("Animation settings")]
    [SerializeField] private float animationSpeed = 1.0f;

    [SerializeField] private AnimationCurve animationCurve;

    [Header("Unity references")]
    [SerializeField] private MeshRenderer cellRenderer;

    [Header("Visibility level materials")]
    [SerializeField] private Material darkCellMat;
    [SerializeField] private Material lightCellMat;
    
    [SerializeField] Material highlightMaterialDarkValid;
    [SerializeField] Material highlightMaterialDarkInvalid;
    
    [SerializeField] Material highlightMaterialLightValid;
    [SerializeField] Material highlightMaterialLightInvalid;

    private bool isHighlighted = false;
    private bool isValidHighlight = true;
    
    private float animationProgress = 0;
    private LIGHT_LEVEL lightLevel = LIGHT_LEVEL.LIGHT;

    private Stone stone;

    private void OnEnable()
    {
        animationProgress = 0;
        lightLevel = LIGHT_LEVEL.LIGHT;
    }

    public void SetStone(Stone stonePrefab)
    {
        stone = Instantiate(stonePrefab, transform);
    }

    public int GetScore()
    {
        if (stone == null) return 0;
        return stone.lightLevel == lightLevel ? 1 : -1;
    }

    private void Update()
    {
        UpdateAnimationTimer();
        var animationValue = animationCurve.Evaluate(animationProgress);
        transform.localScale = new Vector3(animationValue, animationValue, animationValue);
    }

    public void ToggleCell()
    {
        lightLevel = lightLevel == LIGHT_LEVEL.LIGHT ? LIGHT_LEVEL.DARK : LIGHT_LEVEL.LIGHT;
    }

    public void SetHighlight(bool validStatus)
    {
        isHighlighted = true;
    }

    public void ClearHighlight()
    {
        isHighlighted = false;
    }

    private void UpdateAnimationTimer()
    {
        var updatedProgress = animationProgress + Time.deltaTime * (lightLevel == LIGHT_LEVEL.LIGHT ? 1 : -1);
        animationProgress = Mathf.Clamp(updatedProgress, 0, 1);
        if (animationProgress < 0.5f)
        {
            if (!isHighlighted)
            {
                cellRenderer.material = darkCellMat;
                return;
            }
            
            if (isValidHighlight)
            {
                cellRenderer.material = highlightMaterialDarkValid;
                return;
            }
            cellRenderer.material = highlightMaterialDarkInvalid;
        }
        else
        {
            if (!isHighlighted)
            {
                cellRenderer.material = lightCellMat;
                return;
            }
            
            if (isValidHighlight)
            {
                cellRenderer.material = highlightMaterialLightValid;
                return;
            }
            cellRenderer.material = highlightMaterialLightInvalid;
        }
    }
}
