using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "NewGridTogglePattern", menuName = "Game/Grid Toggle Pattern")]
public class GridTogglePattern : ScriptableObject
{
    public bool[,] pattern;

    public int Size => pattern.GetLength(0); // Assuming it's a square pattern
}
