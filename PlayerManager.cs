using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PlayerManager : MonoBehaviour
{
    public GameObject[] players;

    private void Start()
    {
        SetPlayers();
    }

    /// <summary>
    /// Sets players active or inactive depending on the number of players selected
    /// </summary>
    private void SetPlayers()
    {
        players = 
            GameObject.FindGameObjectsWithTag("Player")
            .OrderBy(player => int.Parse(player.name))
            .ToArray();

        for (int i = PlayerInfo.playerCount; i < players.Count(); i++)
        {
            players[i].SetActive(false);
        }
    }
}
