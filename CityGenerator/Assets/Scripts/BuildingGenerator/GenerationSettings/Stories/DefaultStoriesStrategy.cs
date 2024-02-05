using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultStoriesStrategy : StoriesStrategy
{
    public override Story[] GenerateStories(BuildingStrategiesSettings settings, RectInt bounds, int numberOfStrories){
    return new Story[]  { settings.storyStrategy != null ? 
         settings.storyStrategy.GenerateStory(settings, bounds, 1) :
         ((StoryStrategy)ScriptableObject.CreateInstance<DefaultStoryStrategy>()).GenerateStory(settings, bounds, 1)
        };
   }
}
