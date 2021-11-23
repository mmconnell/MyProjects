using UnityEngine;
using System;
using Sirenix.OdinInspector;

[Serializable]
public class UILocationConfiguration
{
    public RectSide rectSide;
    public Coord coord;
    [ShowIf(nameof(coord), Coord.BOTH)]
    public BothResolver bothResolver;

    [ShowIf(nameof(UsePercentage)), Range(0, 100)]
    public int splitPercentage;

    public bool UsePercentage()
    {
        return coord == Coord.BOTH && (bothResolver == BothResolver.MIDXY || bothResolver == BothResolver.MIDYX);
    }
}
