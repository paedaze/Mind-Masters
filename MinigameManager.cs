using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    public bool isPlayable;
    private Vector3 scoreTextOffset = new Vector3(1.6f, -9.25f, 0f);

    [SerializeField] private TMP_Text centerText;
    [SerializeField] private TMP_Text[] scoreDisplays;
    [SerializeField] private TMP_Text timer;

    [SerializeField] private Image transition;

    private AudioSource music;
    [SerializeField] private AudioSource[] sfxs;

    private void Start()
    {
        music = GetComponent<AudioSource>();
        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        UpdateScore();
        TimerPosition();
    }

    /// <summary>
    /// Updates each player's score and the text position
    /// </summary>
    private void UpdateScore()
    {
        for (int i = 0; i < PlayerInfo.playerCount; i++)
        {
            string id = (i + 1).ToString();
            TMP_Text playerIDText = FindObjectsOfType<TMP_Text>().Single(tmpText => tmpText.text.Contains(id) && tmpText.text.Contains("P"));

            scoreDisplays[i].transform.position = playerIDText.transform.position + scoreTextOffset;
            scoreDisplays[i].text = "Score: " + PlayerInfo.scores[(PlayerID)(i + 1)];
        }
    }

    /// <summary>
    /// Sets the timer text position
    /// </summary>
    private void TimerPosition()
    {
        timer.rectTransform.anchoredPosition = new Vector2(3f, -11f);
    }

    /// <summary>
    /// Timer for the game
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator Timer(float time)
    {
        while (time >= 0f)
        {
            timer.text = "Time: " + Mathf.RoundToInt(time).ToString();
            time -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(ConcludeGame());
    }


    /// <summary>
    /// Fades the scene in with an animation
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeIn()
    {
        transition.color = Color.black;

        yield return new WaitForSecondsRealtime(0.75f);

        float t = 0f;

        while (t < 2f)
        {
            transition.color = Color.Lerp(Color.black, Color.clear, (t / 2f));

            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSecondsRealtime(0.75f);

        StartCoroutine(StartingCutscene());
    }

    /// <summary>
    /// Starting cutscene (3..2..1..Go!!) countdown
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartingCutscene()
    {
        for (int i = 0; i <= 3; i++)
        {
            if (i == 3)
            {
                sfxs[1].Play();
                centerText.text = "GO!";
                StartCoroutine(FlashRainbow(centerText));

                yield return new WaitForSecondsRealtime(0.5f);
                break;
            }

            sfxs[0].Play();
            centerText.text = (3 - i).ToString();

            yield return new WaitForSecondsRealtime(0.5f);
        }

        music.Play();
        StartCoroutine(Timer(60f));
        isPlayable = true;
        centerText.text = "";
    }

    /// <summary>
    /// Flashes the text rainbow, stops flashing when the game stars
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private IEnumerator FlashRainbow(TMP_Text t)
    {
        Color[] colors = new Color[] { Color.blue, Color.green, Color.magenta, Color.red, Color.cyan, Color.yellow };

        while (!isPlayable)
        {
            t.color = colors[Random.Range(0, colors.Length)];

            yield return new WaitForSeconds(0.05f);
        }
    }

    /// <summary>
    /// Updates variables and displays "Finish!" on the screen before switching back to the menu screen
    /// </summary>
    /// <returns></returns>
    private IEnumerator ConcludeGame()
    {
        music.Stop();
        sfxs[2].Play();
        centerText.color = Color.green;
        centerText.text = "Finish!";
        isPlayable = false;

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(0);
    }
}
