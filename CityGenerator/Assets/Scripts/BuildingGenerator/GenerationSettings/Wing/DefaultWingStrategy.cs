using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building Generation/Wing/DefaultWing")]
public class DefaultWingStrategy : WingStrategy
{
    public override  Wing GenerateWing(BuildingStrategiesSettings strategySettings, BuildingSettings buildingSettigns, 
        RectInt bounds){
    return new Wing(
        bounds,
        strategySettings.storiesStrategy != null ? 
          strategySettings.storiesStrategy.GenerateStories(strategySettings, buildingSettigns,  bounds) :
          ((StoriesStrategy)ScriptableObject.CreateInstance<DefaultStoriesStrategy>()).GenerateStories(strategySettings, buildingSettigns, bounds),
          strategySettings.roofStrategy != null ? 
          strategySettings.roofStrategy.GenerateRoof(strategySettings, bounds) :
          ((RoofStrategy)ScriptableObject.CreateInstance<DefaultRoofStrategy>()).GenerateRoof(strategySettings, bounds)
    );
   }
}
