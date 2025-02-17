using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;

public class VictoryScreen : MonoBehaviour
{
    public bool animationComplete;
    [SerializeField] private TMP_Text[] playerScores;
    [SerializeField] private TMP_Text header;
    [SerializeField] private AudioSource victorySfx;
    [SerializeField] private AudioSource tallySfx;

    private void OnEnable()
    {
        StartCoroutine(ScoreAnimation());
    }

    /// <summary>
    /// Animates the scoreboard on the victory screen by counting up the numbers individually for each player and displaying the winner at the end
    /// </summary>
    /// <returns></returns>
    private IEnumerator ScoreAnimation()
    {
        for (int i = 0; i <= PlayerInfo.playerCount; i++)
        {
            if (i + 1 > 4 || PlayerInfo.scores[(PlayerID)i + 1] <= 0)
                continue;

            float score = 0;
            PlayerDifficulty diff = PlayerInfo.chosenDifficulty[(PlayerID)i + 1];
            int roundedScore = (int) Mathf.Ceil(PlayerInfo.scores[(PlayerID)i + 1]);

            for (int b = 0; b <= roundedScore; b++)
            {
                score = UpdateScore((PlayerID)i + 1, b, score, roundedScore);

                Instantiate(tallySfx, transform.position, Quaternion.identity, transform).Play();
                yield return new WaitForSeconds(1f / (roundedScore * 2f));
            }

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.5f);

        PlayerID winnerID = FindWinner();
        header.text = "Player " + (int)winnerID + " wins!";

        ResetScores();
        victorySfx.Play();
        StartCoroutine(FlashRainbow(header));
        animationComplete = true;
    }

    /// <summary>
    /// Progress the animation based on certain parameters
    /// </summary>
    /// <param name="player"></param>
    /// <param name="i"></param>
    /// <param name="score"></param>
    /// <param name="roundedScore"></param>
    /// <returns></returns>
    private float UpdateScore(PlayerID id, int i, float score, int roundedScore)
    {
        playerScores[(int)id - 1].text = score.ToString();

        score += 1;
        if (i == roundedScore - 1 && PlayerInfo.scores[id] % 1 != 0)
            score -= 0.5f;

        return score;
    }

    /// <summary>
    /// Finds the winner of the game and returns their PlayerID
    /// </summary>
    /// <returns></returns>
    private PlayerID FindWinner()
    {
        List<PlayerID> winners = new List<PlayerID>();
        float highestScore = PlayerInfo.scores.Values.Max();

        for (int i = 1; i <= PlayerInfo.playerCount; i++)
        {
            if (PlayerInfo.scores[(PlayerID)i] == highestScore)
                winners.Add((PlayerID)i);
        }

        return winners
            .OrderByDescending(id => PlayerInfo.chosenDifficulty[id])
            .First();
    }

    /// <summary>
    /// It resets the fucklign score
    /// </summary>
    private void ResetScores()
    {
        foreach (PlayerID id in Enum.GetValues(typeof(PlayerID)))
        {
            PlayerInfo.scores[id] = 0f;
        }
    }

    /// <summary>
    /// Flashes the text rainbow, stops flashing when the game stars
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private IEnumerator FlashRainbow(TMP_Text t)
    {
        Color[] colors = new Color[] { Color.blue, Color.green, Color.magenta, Color.red, Color.cyan, Color.yellow };

        while (true)
        {
            t.color = colors[UnityEngine.Random.Range(0, colors.Length)];

            yield return new WaitForSeconds(0.05f);
        }
    }
}
