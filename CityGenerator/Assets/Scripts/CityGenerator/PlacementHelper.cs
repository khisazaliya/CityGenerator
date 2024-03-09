using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlacementHelper
{
    public static System.Random rand = new System.Random();
    public static List<Direction> FindNeighbour(Vector3Int position, ICollection<Vector3Int> collection, Vector3Int roadSize)
    {
        List<Direction> neighbourDirections = new List<Direction>();
        if (collection.Contains(position + Vector3Int.right*roadSize))
        {
            neighbourDirections.Add(Direction.Right);
        }
        if (collection.Contains(position - Vector3Int.right* roadSize))
        {
            neighbourDirections.Add(Direction.Left);
        }
        if (collection.Contains(position + new Vector3Int(0, 0, 1)* roadSize))
        {
            neighbourDirections.Add(Direction.Up);
        }
        if (collection.Contains(position - new Vector3Int(0, 0, 1)* roadSize))
        {
            neighbourDirections.Add(Direction.Down);
        }
        return neighbourDirections;
    }

    internal static Vector3Int GetOffsetFromDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Vector3Int(0, 0, (rand.Next(3, 12)));
            case Direction.Down:
                return new Vector3Int(0, 0, (rand.Next(-10, -1)));
            case Direction.Left:
                return new Vector3Int(rand.Next(-10,-1), 0, 0);
            case Direction.Right:
                return new Vector3Int(rand.Next(3, 12), 0, 0);
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
