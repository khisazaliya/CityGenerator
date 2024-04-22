using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roof 
{

   RoofDirection direction;


   public RoofDirection Direction { get => direction; }

}

public enum RoofDirection{
    North, //positive y
    East, //positive x
    South, //negative y
    West //negative x
}
