using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    public static Building Generate(BuildingSettings settings)
    {
        if (settings == null)
        {
            settings = new BuildingSettings();
            settings.buildingSize.x = 1;
            settings.buildingSize.y = 1;
            Debug.Log(settings == null);
            ((WingsStrategy)ScriptableObject.CreateInstance<DefaultWingsStrategy>()).GenerateWings(settings);

        }
        return new Building(
           settings.Size.x,
           settings.Size.y,
           settings.wingsStrategy != null ?
           settings.wingsStrategy.GenerateWings(settings) :
           ((WingsStrategy)ScriptableObject.CreateInstance<DefaultWingsStrategy>()).GenerateWings(settings)
       );
    }
}
