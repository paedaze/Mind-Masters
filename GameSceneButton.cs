using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneButton : SceneButton
{
    protected override void Action()
    {
        scene = (int)PlayerInfo.currentGame;

        base.Action();
    }
}
