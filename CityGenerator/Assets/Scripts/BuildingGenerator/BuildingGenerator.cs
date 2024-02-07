using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    public static Building Generate(BuildingStrategiesSettings strategySettings, BuildingSettings buildingSettings)
    {
        if (strategySettings == null)
        {
            strategySettings = new BuildingStrategiesSettings();
            buildingSettings.buildingSize.x = 1;
            buildingSettings.buildingSize.y = 1;
            Debug.Log(strategySettings == null);
            ((WingsStrategy)ScriptableObject.CreateInstance<DefaultWingsStrategy>()).GenerateWings(strategySettings, buildingSettings);


        }
        return new Building(
           buildingSettings.numberOfLevels,
           buildingSettings.buildingSize.x,
           buildingSettings.buildingSize.y,
           strategySettings.wingsStrategy != null ?
           strategySettings.wingsStrategy.GenerateWings(strategySettings, buildingSettings) :
           ((WingsStrategy)ScriptableObject.CreateInstance<DefaultWingsStrategy>()).GenerateWings(strategySettings, buildingSettings),
           buildingSettings.numberOfEntries
       );
    }
}
