using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultWingsStrategy : WingsStrategy
{
    public override Wing[] GenerateWings(BuildingStrategiesSettings settings, BuildingSettings buildingSettings){
    return new Wing[] { 
        settings.wingStrategy != null ? 
        settings.wingStrategy.GenerateWing(
            settings,
            new RectInt(0, 0 , buildingSettings.buildingSize.x, buildingSettings.buildingSize.y),
            1) :
            ((WingStrategy)ScriptableObject.CreateInstance<DefaultWingStrategy>()).GenerateWing(
                settings,
                new RectInt(0, 0 , buildingSettings.buildingSize.x, buildingSettings.buildingSize.y),
                1 
            )
    };
   }
}