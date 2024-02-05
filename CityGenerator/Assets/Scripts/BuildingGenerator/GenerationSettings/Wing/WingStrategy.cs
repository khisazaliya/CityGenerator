using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WingStrategy : ScriptableObject
{
    public abstract Wing GenerateWing(BuildingStrategiesSettings settings, RectInt bounds, int numberOfStrories);
}
