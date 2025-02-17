using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DifficultySelect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer upArrow;
    [SerializeField] private SpriteRenderer downArrow;

    [SerializeField] private PlayerID id;
    private TMP_Text tMP_Text;
    private int currentIndex;


    private void Start()
    {
        tMP_Text = GetComponent<TMP_Text>();
        currentIndex = 1;
    }

    private void Update()
    {
        if (FindObjectOfType<PageNavigator>().currentlyNavigable)
        {
            Navigate();
        }

        DisableArrowSprite();
        DisplayDifficulty();

        PlayerInfo.chosenDifficulty[id] = (PlayerDifficulty)currentIndex;
    }

    private void Navigate()
    {
        string up = PlayerInfo.inputs[id][0];
        string down = PlayerInfo.inputs[id][2];

        if (Input.GetKeyDown(up))
            currentIndex++;

        if (Input.GetKeyDown(down))
            currentIndex--;

        currentIndex = Mathf.Clamp(currentIndex, 0, 2);
    }

    private void DisableArrowSprite()
    {
        bool lowerEdge = (currentIndex <= 0);
        bool upperEdge = (currentIndex >= 2);

        if (lowerEdge)
        {
            downArrow.color = Color.clear;
        }
        else if (upperEdge)
        {
            upArrow.color = Color.clear;
        }
        else
        {
            downArrow.color = Color.white;
            upArrow.color = Color.white;
        }
    }

    private void DisplayDifficulty()
    {
        string difficulty = Enum.GetName(typeof(PlayerDifficulty), currentIndex);

        tMP_Text.text = difficulty;
        tMP_Text.color = PlayerInfo.difficultyColors[(PlayerDifficulty)currentIndex];
    }
}
