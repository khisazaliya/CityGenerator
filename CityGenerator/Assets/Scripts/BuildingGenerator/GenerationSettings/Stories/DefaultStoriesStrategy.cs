using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building Generation/Stories/DefaultStories")]
public class DefaultStoriesStrategy : StoriesStrategy
{
    public override Story[] GenerateStories(BuildingStrategiesSettings strategySettings, BuildingSettings buildingSettings, RectInt bounds){
        Debug.Log(buildingSettings.numberOfLevels + "truly 2");
         return new Story[]  { strategySettings.storyStrategy != null ? 
         strategySettings.storyStrategy.GenerateStory(strategySettings, buildingSettings, bounds) :
         ((StoryStrategy)ScriptableObject.CreateInstance<DefaultStoryStrategy>()).GenerateStory(strategySettings, buildingSettings, bounds)
        };
   }
}
