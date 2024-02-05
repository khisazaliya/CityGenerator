using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoriesStrategy : ScriptableObject
{
   public abstract Story[] GenerateStories(BuildingStrategiesSettings settings, RectInt bounds, int numberOfStrories);
}
