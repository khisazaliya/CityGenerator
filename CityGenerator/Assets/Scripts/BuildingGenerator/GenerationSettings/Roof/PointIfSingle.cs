using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Building Generation/Roof/Point If Single")]
public class PointIfSingle : RoofStrategy
{
    public override Roof GenerateRoof(BuildingStrategiesSettings settings, RectInt bounds)
    {
        if (bounds.size.x == 1 && bounds.size.y == 1)
        {
            return new Roof(RoofType.Peak);
        }

        else
        {
            System.Array t = typeof(RoofType).GetEnumValues();
            int max = t.Length;
            return new Roof((RoofType)UnityEngine.Random.Range(1, max));
        }
    }
}
