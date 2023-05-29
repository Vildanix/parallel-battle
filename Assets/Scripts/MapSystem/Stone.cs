using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class Stone : MonoBehaviour
{
    [field:SerializeField]public LIGHT_LEVEL lightLevel { get; private set; } = LIGHT_LEVEL.LIGHT;
}
