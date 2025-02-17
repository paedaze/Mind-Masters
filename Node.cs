using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 position;
    public Vector2Int gridPosition;
    public bool isTraversable;
    public Direction dir;

    public Node(Vector3 position, Vector2Int gridPosition)
    {
        this.position = position;
        this.gridPosition = gridPosition;
        this.isTraversable = false;
        dir = Direction.None;
    }

    public void ActivateNode(Direction dir)
    {
        isTraversable = true;
        this.dir = dir;
    }
}

public enum Direction
{
    None = -1,
    Up, 
    Left, 
    Right = 3
}
