using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building 
{
    Vector2Int size;
    Wing[] wings;

    public int level;
    public Vector2Int Size { get { return size; } }
    public Wing[] Wings { get { return wings; } }

    public Building(int numberOfLevels, int sizeX, int sizeY, Wing[] wings) {
        level = numberOfLevels;
        size = new Vector2Int(sizeX, sizeY);
        this.wings = wings;
    }

    public Building(int sizeX, int sizeY)
    {
        size = new Vector2Int(sizeX, sizeY);
    }

    public override string ToString(){
        string bldg = "Building:(" + size.ToString() + ";" + wings.Length + ")\n";
        foreach (var w in wings){
            bldg += "\t" + w.ToString() + "\n";
        }
        return bldg;
    }
}
