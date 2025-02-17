using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardButton : MenuButton
{
    protected override void Action()
    {
        base.Action();

        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && isSelected)
        {
            manager.currentPageNumber = Mathf.Clamp(manager.currentPageNumber + 1, 0, manager.pageCount - 1);
            ResetButton();
        }
    }
}
