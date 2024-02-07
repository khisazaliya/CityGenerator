using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building Generation/Story/DefaultStory")]
public class DefaultStoryStrategy : StoryStrategy
{
   public override Story GenerateStory(BuildingStrategiesSettings strategySettings, BuildingSettings buildingSettings, RectInt bounds){
        Debug.Log(buildingSettings.numberOfLevels + "truely");
    return new Story(
        strategySettings.wallsStrategy != null ?
        strategySettings.wallsStrategy.GenerateWalls(strategySettings, bounds) :
        ((WallsStrategy)ScriptableObject.CreateInstance<DefaultWallsStrategy>()).GenerateWalls(strategySettings, bounds)

    );
   }
}
