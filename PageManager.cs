using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PageManager : MonoBehaviour
{
    public int pageCount;
    public Page[] pages;

    public int currentPageNumber;

    private void Start()
    {
        pageCount = FindObjectsOfType<MenuButton>().Max(butt => butt.page) + 1;
        pages = new Page[pageCount];

        for (int i = 0; i < pageCount; i++)
        {
            IEnumerable<MenuButton> buttons = FindObjectsOfType<MenuButton>().Where(butt => butt.page == i);
            int rowCount = FindRowCount(buttons);

            pages[i] = CreatePage(i, rowCount, buttons);
        }
    }

    private void Update()
    {
        EnablePages();
    }

    /// <summary>
    /// Find the number of rows given a list of buttons by counting the amount of unique y positions
    /// </summary>
    /// <param name="buttons"></param>
    /// <returns></returns>
    private int FindRowCount(IEnumerable<MenuButton> buttons)
    {
        List<float> yPositions = new List<float>();

        // Gets unique y positions
        foreach (MenuButton button in buttons)
        {
            if (!yPositions.Contains(button.transform.position.y))
            {
                yPositions.Add(button.transform.position.y);
            }
        }

        return yPositions.Count;
    }

    /// <summary>
    /// Creates a page based on the page number and grid information
    /// </summary>
    /// <param name="page"></param>
    private Page CreatePage(int pageNumber, int rowCount, IEnumerable<MenuButton> buttons)
    {
        MenuButton[][] grid = new MenuButton[rowCount][];
        buttons = buttons
            .OrderByDescending(butt => butt.transform.position.y)
            .ThenBy(butt => butt.transform.position.x);

        SetGrid();

        // Sets up the grid of buttons based on their x and y position
        void SetGrid()
        {
            int i = 0;
            foreach (MenuButton button in buttons)
            {
                if (grid[i] != null && grid[i][0].transform.position.y != button.transform.position.y)
                {
                    i++;
                }
                
                // Finds buttons with a common y axis and then orders them by their x position
                grid[i] = buttons
                    .Where(butt => butt.transform.position.y == button.transform.position.y)
                    .OrderBy(butt => butt.transform.position.x)
                    .ToArray();

                // Sets each button's grid position after its sorted in the previous code segment
                for (int b = 0; b < grid[i].Length; b++)
                    grid[i][b].gridPosition = new Vector2Int(i, b);
            }
        }

        // Finds the default button for each page: the button to be selected first when loading a page. 
        // The initial default button is set by enabling "isSelected" in the editor
        MenuButton InitialButton()
        {
            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    if (grid[y][x].isSelected)
                        return grid[y][x];
                }
            }

            return grid[0][0];
        }

        // Finds the max number of columns
        int FindMaxColumns()
        {
            int maxX = 0;
            foreach (MenuButton[] arr in grid)
            {
                if (arr.Length > maxX)
                    maxX = arr.Length;
            }

            return maxX;
        }

        return new Page(pageNumber, grid, FindMaxColumns(), rowCount, InitialButton());
    }

    /// <summary>
    /// Enables and disables pages given the current page the player is on
    /// </summary>
    private void EnablePages()
    {
        for (int i = 0; i < pageCount; i++)
        {
            // Enables page if current page, else it disables page
            if (pages[i].pageNumber == currentPageNumber)
            {
                Array.ForEach(pages[i].GetButtons(), butt => butt.gameObject.SetActive(true));
                continue;
            }

            Array.ForEach(pages[i].GetButtons(), butt => butt.gameObject.SetActive(false));
        }
    }
}
