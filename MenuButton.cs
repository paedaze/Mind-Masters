using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public int page;
    public bool isSelected;
    public Vector2Int gridPosition;
    protected SpriteRenderer sprite;
    public AudioSource sfx;

    protected PageManager manager;
    protected PageNavigator navigator;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        manager = FindObjectOfType<PageManager>();
        navigator = FindObjectOfType<PageNavigator>();
    }

    private void Update()
    {
        if (PlayerInfo.inGame && SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
        {
            ReturnFromGame();
            PlayerInfo.inGame = false;
        }

        if (sprite != null)
            SetSpriteColor(sprite);
        else
            SetSpriteColor(GetComponent<Image>());

        if (navigator.currentlyNavigable)
        {
            Action();
        }
    }

    /// <summary>
    /// Action to be taken when button is pressed
    /// </summary>
    protected virtual void Action()
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && isSelected)
        {
            if (sfx != null) sfx.Play();
        }
    }

    /// <summary>
    /// Sets the current button selected to the first button on the next page
    /// </summary>
    protected void ResetButton()
    {
        if (navigator.currentButtonSelected == null)
            return;

        navigator.currentButtonSelected.isSelected = false;
        navigator.currentButtonSelected = manager.pages[manager.currentPageNumber].initialButton;
        navigator.currentButtonSelected.isSelected = true;
    }

    /// <summary>
    /// Sets the sprite color based on whether it is selected
    /// </summary>
    protected void SetSpriteColor(SpriteRenderer sprite)
    {
        if (isSelected)
            sprite.color = Color.white;
        else
            sprite.color = Color.gray;
    }
    protected void SetSpriteColor(Image image)
    {
        if (isSelected)
            image.color = Color.white;
        else
            image.color = Color.gray;
    }

    /// <summary>
    /// Takes the player to the victory screen when a game has finished
    /// </summary>
    private void ReturnFromGame()
    {
        manager.currentPageNumber = Mathf.Clamp(15, 0, manager.pageCount);
        ResetButton();
    }

    public override string ToString()
    {
        return gameObject.name + " : " + gridPosition;
    }
}
