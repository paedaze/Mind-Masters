using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSelectButton : PageButton
{
    [SerializeField] private Game game;

    protected override void Action()
    {
        PlayerInfo.currentGame = game;

        pageToGo = 6 + (int)game;
        base.Action();  
    }
}
