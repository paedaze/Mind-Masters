using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PageNavigator : MonoBehaviour
{
    public MenuButton currentButtonSelected;
    public bool currentlyNavigable;
    private PageManager manager;
    private Dictionary<string, Vector2Int> dirs = new Dictionary<string, Vector2Int>
    {
        {"w", new Vector2Int(0, -1)},
        {"a", new Vector2Int(-1, 0)},
        {"s", new Vector2Int(0, 1)},
        {"d", new Vector2Int(1, 0)}
    };

    private void Start()
    {
        currentlyNavigable = true;
        manager = GetComponent<PageManager>();
    }

    private void Update()
    {
        if (!currentlyNavigable)
            return;

        PageNavigation();
    }

    /// <summary>
    /// Handles navigation of menus
    /// </summary>
    private void PageNavigation()
    {
        Page currentPage = manager.pages[manager.currentPageNumber];
        currentButtonSelected = currentPage.GetButtons().Single(butt => butt.isSelected);

        if (!dirs.ContainsKey(Input.inputString))
            return;

        int y = currentButtonSelected.gridPosition.x;
        int x = currentButtonSelected.gridPosition.y;

        NavigateToButton();

        void NavigateToButton()
        {
            // Clamps movement vector to account for the jagged array
            Vector2Int move = dirs[Input.inputString];
            y = Mathf.Clamp(y + move.y, 0, currentPage.maxY - 1);
            x = Mathf.Clamp(x + move.x, 0, currentPage.buttonGrid[y].Length - 1);

            // Find new button 
            currentButtonSelected.isSelected = false;
            currentButtonSelected = currentPage.buttonGrid[y][x];
            currentButtonSelected.isSelected = true;
        }
    }
}
