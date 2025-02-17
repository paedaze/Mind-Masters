using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public struct Page
{
    public MenuButton[][] buttonGrid;
    public MenuButton initialButton;
    public int maxX;
    public int maxY;
    public int pageNumber;

    public Page(int pageNumber, MenuButton[][] buttonGrid, int maxX, int maxY, MenuButton firstButton)
    {
        this.buttonGrid = buttonGrid;
        this.pageNumber = pageNumber;
        this.maxX = maxX;
        this.maxY = maxY;
        initialButton = firstButton;
    }

    /// <summary>
    /// Gets all buttons in the grid
    /// </summary>
    /// <returns></returns>
    public MenuButton[] GetButtons()
    {
        List<MenuButton> buttons = new List<MenuButton>();

        foreach (MenuButton[] arr in buttonGrid)
        {
            Array.ForEach(arr, butt => buttons.Add(butt));
        }

        return buttons.ToArray();
    }

    public override string ToString()
    {
        string output = "";
        foreach (MenuButton[] arr in buttonGrid)
        {
            output += string.Concat<MenuButton>(arr);
            output += "\n";
        }

        return output;
    }
}
