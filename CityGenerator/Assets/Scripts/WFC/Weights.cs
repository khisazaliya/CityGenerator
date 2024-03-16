using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public struct Weights
{
    [Range(0,10)] public int crossroadWeight;
    [Range(0,10)] public int roadStraightWeight;
    [Range(0, 10)] public int cornerWeight;
    [Range(0,10)] public int buildingWeight;
    public int GetWeight(Attribute a)
    {
        if(a==Attribute.Crossroad)
            return crossroadWeight;
        else if(a==Attribute.RoadStraight)
            return roadStraightWeight;
        else if (a == Attribute.Corner)
            return cornerWeight;
        else if(a==Attribute.Building)
            return buildingWeight;


        return 0;
    }
}
public enum Attribute {Crossroad, RoadStraight,Corner,  Building};
