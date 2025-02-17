using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageButton : MenuButton
{
    [SerializeField] protected int pageToGo;

    protected override void Action()
    {
        base.Action();

        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && isSelected)
        {
            manager.currentPageNumber = Mathf.Clamp(pageToGo, 0, manager.pageCount);
            ResetButton();
        }
    }
}
