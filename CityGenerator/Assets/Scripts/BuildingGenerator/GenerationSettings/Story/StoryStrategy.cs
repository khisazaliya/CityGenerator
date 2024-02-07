using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoryStrategy : ScriptableObject
{
    public abstract Story GenerateStory(BuildingStrategiesSettings strategySettings, 
        BuildingSettings buildingSettings, RectInt bounds);
}
