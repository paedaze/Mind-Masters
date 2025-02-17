using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using System;

public class MemoryPlayer : MonoBehaviour
{
    [SerializeField] private Bulb[] bulbs;
    public PlayerID id;
    private PlayerDifficulty diff;
    private MinigameManager manager;
    private bool hasStarted;

    [SerializeField] private TMP_Text x;
    [SerializeField] private TMP_Text difficultyLabel;

    public bool canPlay;
    private Sequence sequence;

    private SoundManager sfxManager;

    private MemoryAnimator anim;

    private void Start()
    {
        sfxManager = FindObjectOfType<SoundManager>();
        anim = GetComponentInChildren<MemoryAnimator>();

        id = (PlayerID)int.Parse(transform.parent.name);
        diff = PlayerInfo.chosenDifficulty[id];
        manager = FindObjectOfType<MinigameManager>();

        for (int i = 0; i < bulbs.Length; i++)
        {
            bulbs[i].key = PlayerInfo.inputs[id][i];
            bulbs[i].id = id;
        }

        x.gameObject.SetActive(false);
        SetDifficultyLabel();

        hasStarted = false;
    }

    private void Update()
    {
        if (manager.isPlayable && !hasStarted)
        {
            StartGame();
            hasStarted = true;
        }
        else if (!manager.isPlayable)
        {
            return;
        }


        // Correctly guessing the sequence
        if (sequence.isCompleted)
        {
            Instantiate(sfxManager.sfxs[1], transform.position, Quaternion.identity, sfxManager.transform).Play();
            PlayerInfo.scores[id] += PlayerInfo.difficultyScale[diff];
            sequence = new Sequence(id);

            anim.StopAllCoroutines();
            StartCoroutine(anim.Win());

            StartCoroutine(SucessAnimation());
        }

        // Prevents the player from guessing while the sequence is playing
        if (!canPlay)
            return;

        // Checks the player's input to see if it was correct
        bool? guess = PlayerGuess();
        if (guess == false)
        {
            Instantiate(sfxManager.sfxs[0], transform.position, Quaternion.identity, sfxManager.transform).Play();
            sequence.ResetSequence();

            anim.StopAllCoroutines();
            StartCoroutine(anim.Fail());

            StartCoroutine(FailAnimation());
        }
    }

    /// <summary>
    /// Starts the game
    /// </summary>
    private void StartGame()
    {
        sequence = new Sequence(id);
        StartCoroutine(PlaySequence(sequence));
    }

    /// <summary>
    /// Sets the difficulty label to the player's corresponding difficulty
    /// </summary>
    private void SetDifficultyLabel()
    {
        difficultyLabel.text = Enum.GetName(typeof(PlayerDifficulty), diff);
        difficultyLabel.color = PlayerInfo.difficultyColors[diff];
    }

    /// <summary>
    /// Checks the players guess 
    /// </summary>
    /// <returns></returns>
    private bool? PlayerGuess()
    {
        string input = Input.inputString;
        CheckArrowInputs();

        if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        if (!Input.GetKeyDown(input))
            return null;

        if (PlayerInfo.inputs[id].Contains(input))
        {
            anim.StopAllCoroutines();
            StartCoroutine(anim.Press());
            return sequence.Guess(input);
        }

        return null;

        void CheckArrowInputs()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                input = "up";
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                input = "left";
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                input = "down";
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                input = "right";
        }
    }

    /// <summary>
    /// Plays the memory sequence for the player by lighting up the bulbs
    /// </summary>
    /// <param name="sequence"></param>
    /// <returns></returns>
    private IEnumerator PlaySequence(Sequence sequence)
    {
        canPlay = false;

        int length = sequence.sequence.Length;
        
        for (int i = 0; i < length; i++)
        {
            Bulb bulb = bulbs.Single(bulb => bulb.key == sequence.sequence[i]);
            StartCoroutine(bulb.ActivateBulb());

            if (i != length - 1)
                yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSecondsRealtime(0.25f);

        canPlay = true;
    }

    /// <summary>
    /// Plays when the player fails to complete the sequence correctly
    /// </summary>
    /// <returns></returns>
    private IEnumerator FailAnimation()
    {
        canPlay = false;

        x.color = Color.red;
        x.text = "X";

        x.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(0.45f);

        x.gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(0.45f);

        StartCoroutine(PlaySequence(sequence));
    }

    /// <summary>
    /// Plays when the player sucessfully completes a sequence
    /// </summary>
    /// <returns></returns>
    private IEnumerator SucessAnimation()
    {
        canPlay = false;

        x.color = Color.green;
        x.text = "./";

        yield return new WaitForSecondsRealtime(0.25f);

        x.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(0.45f);

        x.gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(0.45f);

        StartCoroutine(PlaySequence(sequence));
    }
}
