using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Linq;

public class Notification : MonoBehaviour
{
    private TMP_Text notification;
    private PageManager manager;
    private float timer;

    private void Start()
    {
        manager = FindObjectOfType<PageManager>();
        notification = GetComponent<TMP_Text>();
        StartCoroutine(Timer(0f));
    }

    private void Update()
    {
        notification.fontSize = 45f;

        if (manager.currentPageNumber == 15 || manager.currentPageNumber < 12)
        {
            InformControls();
        }
        else
        {
            notification.fontSize = 35f;
            notification.text = "Use    to select difficulty";
        }
    }

    /// <summary>
    /// Sets the notifcation text to inform the user of the controls for the menu
    /// </summary>
    private void InformControls()
    {
        string input = Input.inputString;
        bool mouseInput = GetMouseInput();
        CheckArrowInputs();
        if (string.IsNullOrEmpty(input) && !mouseInput) return;

        bool wasdInput = PlayerInfo.inputs[PlayerID.Player1].Contains(input);

        if (GetMouseInput() || !(wasdInput || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return)))
        {
            timer = 4f;
            notification.text = "Use WASD and Space/Enter to navigate the menu";
        }

        void CheckArrowInputs()
        {
            if (Input.GetKey(KeyCode.UpArrow))
                input = "up";
            else if (Input.GetKey(KeyCode.LeftArrow))
                input = "left";
            else if (Input.GetKey(KeyCode.DownArrow))
                input = "down";
            else if (Input.GetKey(KeyCode.RightArrow))
                input = "right";
        }

        bool GetMouseInput()
        {
            return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);
        }
    }

    /// <summary>
    /// Handles the time in which the notification is on the screen
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    private IEnumerator Timer(float seconds)
    {
        timer = seconds;

        while (true)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;

                yield return new WaitForEndOfFrame();
                continue;
            }

            notification.text = "";

            yield return new WaitForEndOfFrame();
        }
    }
}
