﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class BuildingSettings
{
    [Range(1, 50)] 
    public int x;

    [Range(1, 50)] 
    public int y;
    public Vector2Int buildingSize => new Vector2Int(x, y);

    public int buildingCount;
    [Range(0, 50)]
    public int numberOfLevels;
    public BuildingType type;

    [Range(0, 50)]
    public int numberOfEntries;

    [Range(0, 50)]
    public int numberOfBalconies;

    public BuildingSettings(Vector2Int size, int count, int numberOfLevels, BuildingType type, int numberOfEntries, int numberOfBalconies)
    {
        x = size.x; 
        y = size.y; 
        buildingCount = count;
        this.numberOfLevels = numberOfLevels;
        this.type = type;
        this.numberOfEntries = numberOfEntries;
        this.numberOfBalconies = numberOfBalconies;
    }
}
