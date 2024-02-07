using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building Generation/Walls/DefaultWalls")]
public class DefaultWallsStrategy : WallsStrategy
{
    public override Wall[] GenerateWalls(BuildingStrategiesSettings settings, RectInt bounds){
    return new Wall[(bounds.size.x + bounds.size.y)* 2];
   }
}
