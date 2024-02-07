using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WingStrategy : ScriptableObject
{
    public abstract Wing GenerateWing(BuildingStrategiesSettings strategySettings, BuildingSettings buildingSettings, RectInt bounds);
}
