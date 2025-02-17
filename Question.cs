using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Question
{
    public Operator op;
    public int first;
    public int second;
    public PlayerDifficulty difficulty;
    public int answer;
    public bool isAnswered;

    public Question(PlayerDifficulty difficulty)
    {
        this.difficulty = difficulty;
        (Vector3Int, Operator) info = (new Vector3Int(0, 0), Operator.Addition);

        switch (difficulty)
        {
            case PlayerDifficulty.Easy:
                info = EasyQuestion();
                break;
            case PlayerDifficulty.Medium:
                info = MediumQuestion();
                break;
            case PlayerDifficulty.Hard:
                info = HardQuestion();
                break;
        }

        this.first = info.Item1.x;
        this.second = info.Item1.y;
        this.answer = info.Item1.z;
        this.op = info.Item2;
        isAnswered = false;
    }

    /// <summary>
    /// Creates a set of answer choices and a question for the desired difficulty
    /// </summary>
    /// <returns></returns>
    private static (Vector3Int, Operator op) EasyQuestion()
    {
        int first = 0;
        int second = 0;
        int answer = 0;
        Operator op = (Operator)Random.Range(0, 2);

        if (op == Operator.Addition)
        {
            first = Random.Range(0, 12);
            second = Random.Range(0, 12);
            answer = first + second;
        }
        else if (op == Operator.Subtraction)
        {
            first = Random.Range(0, 12);
            second = Random.Range(0, first);
            answer = first - second;
        }

        return (new Vector3Int(first, second, answer), op);
    }

    private static (Vector3Int, Operator op) MediumQuestion()
    {
        int first = 0;
        int second = 0;
        int answer = 0;
        Operator op = (Operator)Random.Range(0, 3);

        if (op == Operator.Addition)
        {
            first = Random.Range(5, 15);
            second = Random.Range(5, 15);
            answer = first + second;
        }
        else if (op == Operator.Subtraction)
        {
            first = Random.Range(5, 30);
            second = Random.Range(0, first);
            answer = first - second;
        }
        else if (op == Operator.Multiplication)
        {
            first = Random.Range(0, 12);
            second = Random.Range(0, 12);
            answer = first * second;
        }

        return (new Vector3Int(first, second, answer), op);
    }

    private static (Vector3Int, Operator op) HardQuestion()
    {
        int first = 0;
        int second = 0;
        int answer = 0;
        Operator op = (Operator)Random.Range(0, 4);

        if (op == Operator.Addition)
        {
            first = Random.Range(20, 50);
            second = Random.Range(20, 50);
            answer = first + second;
        }
        else if (op == Operator.Subtraction)
        {
            first = Random.Range(20, 40);
            second = Random.Range(0, 40);
            answer = first - second;
        }
        else if (op == Operator.Multiplication)
        {
            first = Random.Range(6, 12);
            second = Random.Range(6, 12);
            answer = first * second;
        }
        else if (op == Operator.Division)
        {
            first = Random.Range(6, 12);
            second = first * Random.Range(6, 12);

            int temp = first;
            first = second;
            second = temp;

            answer = first / second;
        }

        return (new Vector3Int(first, second, answer), op);
    }

    public override string ToString()
    {
        return first + " " + PlayerInfo.signs[op] + " " + second;
    }
}

public enum Operator
{
    Addition,
    Subtraction,
    Multiplication,
    Division
}
