using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectButton : ForwardButton
{
    [Range(2, 4)]
    [SerializeField] private int count;

    protected override void Action()
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && isSelected)
        {
            PlayerInfo.playerCount = count;
        }

        base.Action();
    }
}
