using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : PageButton
{
    protected override void Action()
    {
        pageToGo = 10 + PlayerInfo.playerCount;

        base.Action();
    }
}
