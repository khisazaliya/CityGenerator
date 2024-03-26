/*using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PlacementHelper
{
    public static System.Random rand = new System.Random();
    public static List<Direction> FindNeighbour(Vector3Int position, ICollection<Vector3Int> collection, int roadModuleSize)
    {
        List<Direction> neighbourDirections = new List<Direction>();
    
        // ѕровер€ем соседние клетки по горизонтали
        if (collection.Contains(position + Vector3Int.right * roadModuleSize))
        {
            neighbourDirections.Add(Direction.Right);
        }
        if (collection.Contains(position - Vector3Int.right * roadModuleSize))
        {
            neighbourDirections.Add(Direction.Left);
        }

        // ѕровер€ем соседние клетки по вертикали
        if (collection.Contains(position + new Vector3Int(0, 0, roadModuleSize)))
        {
            neighbourDirections.Add(Direction.Up);
        }
        if (collection.Contains(position - new Vector3Int(0, 0, roadModuleSize)))
        {
            neighbourDirections.Add(Direction.Down);
        }

        return neighbourDirections;
    }



    internal static Vector3Int GetOffsetFromDirection(Direction direction, int roadScale)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Vector3Int(0, 0, (rand.Next(1 + roadScale, 10 + roadScale)));
            case Direction.Down:
                return new Vector3Int(0, 0, (rand.Next(-10 - roadScale, -1 - roadScale)));
            case Direction.Left:
                return new Vector3Int(rand.Next(-10 - roadScale, -1 - roadScale), 0, 0);
            case Direction.Right:
                return new Vector3Int(rand.Next(1 + roadScale, 10 + roadScale), 0, 0);
            default:
                break;
        }
        throw new System.Exception("No direction such as " + direction);
    }

    public static Direction GetReverseDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            default:
                break;
        }
        throw new System.Exception("No direction such as " + direction);
    }
}
*/