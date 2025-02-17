using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Sequence
{
    private PlayerID id;
    private PlayerDifficulty diff;
    public string[] sequence;
    public bool isCompleted;
    public int index;

    public Sequence(PlayerID id)
    {
        this.id = id;
        this.index = 0;
        this.diff = PlayerInfo.chosenDifficulty[id];
        this.isCompleted = false;

        sequence = new string[PlayerInfo.sequenceLengths[diff]];
        GenerateSequence();
    }

    private void GenerateSequence()
    {
        for (int i = 0; i < sequence.Length; i++)
        {
            sequence[i] = PlayerInfo.inputs[id][Random.Range(0, 4)];
        }
    }

    public bool Guess(string input)
    {
        bool isCorrect = sequence[index] == input;
        index++;

        if (index == sequence.Length && isCorrect)
            isCompleted = true;

        return isCorrect;
    }

    public void ResetSequence()
    {
        index = 0;
        isCompleted = false;
    }
}
