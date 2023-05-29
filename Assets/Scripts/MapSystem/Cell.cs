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
    
    private float animationProgress = 0;
    private LIGHT_LEVEL lightLevel = LIGHT_LEVEL.LIGHT;

    private enum LIGHT_LEVEL { LIGHT = 1, DARK = 2};

    private void OnEnable()
    {
        animationProgress = 0;
        lightLevel = LIGHT_LEVEL.LIGHT;
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

    public void SetHighlight() {
        
    }

    public void ClearHighlight() {

    }

    private void UpdateAnimationTimer()
    {
        var updatedProgress = animationProgress + Time.deltaTime * (lightLevel == LIGHT_LEVEL.LIGHT ? -1 : 1);
        animationProgress = Mathf.Clamp(updatedProgress, 0, 1);
        if (animationProgress < 0.5f)
        {
            cellRenderer.material = darkCellMat;
        }
        else
        {
            cellRenderer.material = lightCellMat;
        }
    }
}
