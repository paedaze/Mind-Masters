using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public struct Maze
{
    public Node[][] nodes;
    public bool isCompleted;
    public List<Node> nodeSequence;
    public List<Direction> sequence;

    private Vector3 origin;
    private PlayerDifficulty diff;

    public Maze(PlayerDifficulty diff, Vector3 origin)
    {
        this.diff = diff;
        this.origin = origin;
        isCompleted = false;
        nodes = new Node[PlayerInfo.length][];
        sequence = new List<Direction>();
        nodeSequence = new List<Node>();

        while (sequence.Count != PlayerInfo.mazeInputLengths[diff])
        {
            sequence = new List<Direction>();
            nodeSequence = new List<Node>();
            GenerateMaze();
        }
    }

    /// <summary>
    /// Generates a random maze on instantiation
    /// </summary>
    private void GenerateMaze()
    {
        for (int y = 0; y < PlayerInfo.length; y++)
        {
            Vector3 position = origin + (Vector3.left * 11f) + (Vector3.up * y);
            nodes[y] = new Node[PlayerInfo.width];

            for (int x = 0; x < PlayerInfo.width; x++)
            {
                nodes[y][x] = new Node(position, new Vector2Int(x, y));
                position += Vector3.right;
            }
        }

        GenerateRoute(11, diff);
    }

    /// <summary>
    /// Handles the logic for generating a random maze
    /// </summary>
    /// <param name="x"></param>
    /// <param name="diff"></param>
    private void GenerateRoute(int x, PlayerDifficulty diff)
    {
        Vector2Int gridPos = new Vector2Int(x, 0);
        int index = PlayerInfo.mazeSequenceLengths[diff] - 1;

        for (int i = index; i > 0; i--)
        {
            // Handles column creation
            int length = UnityEngine.Random.Range(1, Mathf.Clamp(PlayerInfo.length - i - gridPos.y, 1, int.MaxValue));
            if (i == 1)
                length = PlayerInfo.length - gridPos.y;

            gridPos += SetColumn(gridPos, length);

            if (i == 1)
                break;

            // Handles row creation
            Direction dir;
            int rowLength;

            if (i == 2)
            {
                if (gridPos.x > 11)
                {
                    dir = Direction.Left;
                    rowLength = gridPos.x - 11;
                }
                else if (gridPos.x < 11)
                {
                    dir = Direction.Right;
                    rowLength = 11 - gridPos.x;
                }
                else return;

                gridPos += SetRow(gridPos, rowLength, dir);
                continue;
            }

            dir = PlayerInfo.horizontalDirs[UnityEngine.Random.Range(0, 2)];
            if (gridPos.x == 4) dir = Direction.Right;
            if (gridPos.x == 17) dir = Direction.Left;

            if (dir == Direction.Left)
                rowLength = UnityEngine.Random.Range(1, gridPos.x - 4 + 1);
            else
                rowLength = UnityEngine.Random.Range(1, 17 - gridPos.x + 1);

            gridPos += SetRow(gridPos, rowLength, dir);
        }
    }

    /// <summary>
    /// Creates a new column from existing nodes by activating them
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    private Vector2Int SetColumn(Vector2Int gridPos, int length)
    {
        for (int i = 0; i < length; i++)
        {
            int y = gridPos.y + i;
            int x = gridPos.x;

            nodeSequence.Add(nodes[y][x]);
            nodes[y][x].ActivateNode(Direction.Up);
        }

        sequence.Add(Direction.Up);

        return Vector2Int.up * length;
    }

    /// <summary>
    /// Creates a row from existing nodes by activating them
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="length"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    private Vector2Int SetRow(Vector2Int gridPos, int length, Direction dir)
    {
        for (int i = 0; i < length; i++)
        {
            int y = gridPos.y;

            int xDir = dir == Direction.Left ? -i : i;
            int x = gridPos.x + xDir;

            nodeSequence.Add(nodes[y][x]);
            nodes[y][x].ActivateNode(dir);
        }

        sequence.Add(dir);

        if (dir == Direction.Left)
        {
            return Vector2Int.left * length;
        }

        return Vector2Int.right * length;
    }


    /// <summary>
    /// Gets the neighbors of a node at the given grid index
    /// </summary>
    /// <param name="gridPos"></param>
    /// <returns></returns>
    public Node GetLowerNode(Vector2Int gridPos)
    {
        Node node = null;
        try
        {
            node = nodes[gridPos.y - 1][gridPos.x];
        }
        catch (Exception) { }

        return node;
    }
}
