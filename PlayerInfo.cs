using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInfo
{
    public static int playerCount = 4;
    public static Game currentGame = 0;
    public static bool inGame = false;

    #region Camera Setup

    public static Vector2[] twoPlayerSetup = 
    { 
        new Vector2(0f, 0.5f),
        new Vector2(0f, 0f) 
    };
    public static Vector2[] threePlayerSetup = 
    { 
        new Vector2(0f, 0.5f), 
        new Vector2(0.5f, 0.5f),
        new Vector2(0.125f, 0f) 
    };
    public static Vector2[] fourPlayerSetup = 
    { 
        new Vector2(0f, 0.5f),
        new Vector2(0.5f, 0.5f),
        new Vector2(0f, 0f),
        new Vector2(0.5f, 0f) 
    };

    #endregion

    #region Player Info

    private static string[] player1Inputs = { "w", "a", "s", "d" };
    private static string[] player2Inputs = { "t", "f", "g", "h" };
    private static string[] player3Inputs = { "i", "j", "k", "l" };
    private static string[] player4Inputs = { "up", "left", "down", "right" };
    public static Dictionary<PlayerID, string[]> inputs = new Dictionary<PlayerID, string[]>
    {
        {PlayerID.Player1, player1Inputs},
        {PlayerID.Player2, player2Inputs},
        {PlayerID.Player3, player3Inputs},
        {PlayerID.Player4, player4Inputs}
    };

    public static Dictionary<PlayerID, PlayerDifficulty> chosenDifficulty = new Dictionary<PlayerID, PlayerDifficulty>
    {
        {PlayerID.Player1, PlayerDifficulty.Medium },
        {PlayerID.Player2, PlayerDifficulty.Medium },
        {PlayerID.Player3, PlayerDifficulty.Medium },
        {PlayerID.Player4, PlayerDifficulty.Medium },
    };

    public static Dictionary<PlayerID, float> scores = new Dictionary<PlayerID, float>
    {
        { PlayerID.Player1, 0f },
        { PlayerID.Player2, 0f },
        { PlayerID.Player3, 0f },
        { PlayerID.Player4, 0f },
    };

    #endregion

    #region Difficulty Info

    public static Dictionary<PlayerDifficulty, Color> difficultyColors = new Dictionary<PlayerDifficulty, Color>
    {
        { PlayerDifficulty.Easy, Color.green },
        { PlayerDifficulty.Medium, Color.yellow },
        { PlayerDifficulty.Hard, Color.red }
    };
    public static Dictionary<PlayerDifficulty, float> difficultyScale = new Dictionary<PlayerDifficulty, float>
    {
        { PlayerDifficulty.Easy, 1f },
        { PlayerDifficulty.Medium, 1.5f },
        { PlayerDifficulty.Hard, 2f }
    };

    #endregion

    #region Math Game

    public static Dictionary<Operator, string> signs = new Dictionary<Operator, string>
    {
        { Operator.Addition, "+" },
        { Operator.Subtraction, "-" },
        { Operator.Multiplication, "x" },
        { Operator.Division, "/" },
    };
    public static Dictionary<PlayerDifficulty, float> cooldowns = new Dictionary<PlayerDifficulty, float>
    {
        { PlayerDifficulty.Easy, 0.5f },
        { PlayerDifficulty.Medium, 1f },
        { PlayerDifficulty.Hard, 2f }
    };

    #endregion

    #region Memory Game

    public static Dictionary<PlayerDifficulty, int> sequenceLengths = new Dictionary<PlayerDifficulty, int>
    {
        { PlayerDifficulty.Easy, 4 },
        { PlayerDifficulty.Medium, 5 },
        { PlayerDifficulty.Hard, 6 }
    };

    public static Direction[] horizontalDirs = { Direction.Left, Direction.Right };

    #endregion

    #region Maze Game

    public static Dictionary<PlayerDifficulty, int> mazeSequenceLengths = new Dictionary<PlayerDifficulty, int>
    {
        { PlayerDifficulty.Easy, 4 },
        { PlayerDifficulty.Medium, 6 },
        { PlayerDifficulty.Hard, 8 }
    };
    public static Dictionary<PlayerDifficulty, int> mazeInputLengths = new Dictionary<PlayerDifficulty, int>
    {
        { PlayerDifficulty.Easy, 5 },
        { PlayerDifficulty.Medium, 9 },
        { PlayerDifficulty.Hard, 13 }
    };

    public static int width = 23;
    public static int length = 10;

    #endregion

}

public enum PlayerID
{
    Player1 = 1,
    Player2,
    Player3, 
    Player4
}

public enum PlayerDifficulty
{
    Easy,
    Medium,
    Hard
}

public enum Game
{
    MathGame = 1,
    MemoryGame,
    MazeGame
}
