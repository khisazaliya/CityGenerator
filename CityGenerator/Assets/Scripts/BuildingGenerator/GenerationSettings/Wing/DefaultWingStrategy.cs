using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultWingStrategy : WingStrategy
{
    public override  Wing GenerateWing(BuildingStrategiesSettings settings, RectInt bounds, int numberOfStrories){
    return new Wing(
        bounds,
        settings.storiesStrategy != null ? 
          settings.storiesStrategy.GenerateStories(settings, bounds, numberOfStrories) :
          ((StoriesStrategy)ScriptableObject.CreateInstance<DefaultStoriesStrategy>()).GenerateStories(settings, bounds, numberOfStrories),
          settings.roofStrategy != null ? 
          settings.roofStrategy.GenerateRoof(settings, bounds) :
          ((RoofStrategy)ScriptableObject.CreateInstance<DefaultRoofStrategy>()).GenerateRoof(settings, bounds)
    );
   }
}
