using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Building Generation/Wings/DefaultWings")]
public class DefaultWingsStrategy : WingsStrategy
{
    public override Wing[] GenerateWings(BuildingStrategiesSettings strategySettings, BuildingSettings buildingSettings){
    return new Wing[] { 
        strategySettings.wingStrategy != null ? 
        strategySettings.wingStrategy.GenerateWing(
            strategySettings,
            buildingSettings,
            new RectInt(0, 0 , buildingSettings.buildingSize.x, buildingSettings.buildingSize.y)
            ) :
            ((WingStrategy)ScriptableObject.CreateInstance<DefaultWingStrategy>()).GenerateWing(
                strategySettings,
                buildingSettings,
                new RectInt(0, 0 , buildingSettings.buildingSize.x, buildingSettings.buildingSize.y)
            )
    };
   }
}