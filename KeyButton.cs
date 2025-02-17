using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyButton : MenuButton
{
    [SerializeField] private KeyCode key;

    protected override void Action()
    {
        if (Input.GetKeyDown(key) && isSelected)
        {
            int moveDir = manager.currentPageNumber <= 0 ? 1 : -1;
            manager.currentPageNumber = Mathf.Clamp(manager.currentPageNumber + moveDir, 0, manager.pageCount - 1);

            ResetButton();
        }
    }
}
