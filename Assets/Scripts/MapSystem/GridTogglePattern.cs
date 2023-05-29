using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "NewGridTogglePattern", menuName = "Game/Grid Toggle Pattern")]
public class GridTogglePattern : ScriptableObject
{
    public bool[] patternTop;
    public bool[] patternSide;
    public bool[] patternBottom;

    public int Size => patternTop.GetLength(0); // Assuming it's a square pattern
}
