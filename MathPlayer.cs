using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class MathPlayer : MonoBehaviour
{
    [SerializeField] public PlayerID id;
    [SerializeField] private TMP_Text difficultyLabel;
    [SerializeField] private TMP_Text questionLabel;
    [SerializeField] private TMP_Text x;

    #region Miscellaneous

    private SoundManager sfxManager;
    private MathAnimator animator;
    private IEnumerator failCoroutine;

    #endregion

    #region Node Assets

    private GameObject startingNode;
    private GameObject node;

    #endregion

    #region Node Spawning

    private float size;
    private Vector3 nodeOffset;
    private Vector3 offset;
    private Transform nextNode;
    private GameObject nextNextNode;

    #endregion 

    #region Math Question

    [SerializeField] private GameObject[] choiceDisplays;

    private Question question;
    [SerializeField] private int[] choices;
    [SerializeField] private float cooldown;
    private Dictionary<string, int> answerBox;

    #endregion 

    private void Start()
    {
        sfxManager = FindObjectOfType<SoundManager>();
        animator = GetComponentInChildren<MathAnimator>();

        startingNode = GameObject.FindGameObjectWithTag("Starting Node");
        node = GameObject.FindGameObjectWithTag("Node");

        size = GetComponent<BoxCollider2D>().size.y;
        offset = Vector3.down * (size * 0.5f);
        nodeOffset = Vector3.up * 6f;

        SetDifficultyLabel();
        GenerateStartingPlatforms();

        question = new Question(PlayerInfo.chosenDifficulty[id]);
        choices = new int[3];
        cooldown = 0f;
        ScrambleChoices();

        x.gameObject.SetActive(false);
        StartCoroutine(Cooldown());
    }

    private void Update()
    {
        SetQuestion();

        if (!FindObjectOfType<MinigameManager>().isPlayable)
            return;

        CheckAnswer();
        if (question.isAnswered)
        {
            question = new Question(PlayerInfo.chosenDifficulty[id]);
            ScrambleChoices();

            if (nextNextNode != null)
                Destroy(nextNextNode.gameObject);

            nextNode = GenerateNextPlatform(0).transform;
            nextNextNode = GenerateNextPlatform(1);

            StartCoroutine(Move());

            PlayerInfo.scores[id] += PlayerInfo.difficultyScale[PlayerInfo.chosenDifficulty[id]];
        }
    }

    // UI and asset scripts
    /// <summary>
    /// Sets the color and text of the difficulty label according to the difficulty the player selected
    /// </summary>
    private void SetDifficultyLabel()
    {
        // The dictionary values are set by the player in the start screen
        PlayerDifficulty difficulty = PlayerInfo.chosenDifficulty[id];

        difficultyLabel.text = Enum.GetName(typeof(PlayerDifficulty), difficulty);
        difficultyLabel.color = PlayerInfo.difficultyColors[difficulty];
    }

    /// <summary>
    /// Generates the first platforms at the start of the game
    /// </summary>
    private void GenerateStartingPlatforms()
    {
        Instantiate(startingNode, transform.position + offset, Quaternion.identity);

        nextNode = Instantiate(node, transform.position + offset + nodeOffset, Quaternion.identity).transform;
    }

    /// <summary>
    /// Generates the next platform
    /// </summary>
    private GameObject GenerateNextPlatform(float index)
    {
        return Instantiate(node, transform.position + offset + (nodeOffset * (index + 1)), Quaternion.identity);
    }

    // Game Scripts
    /// <summary>
    /// Updates the question information displayed to the player
    /// </summary>
    private void SetQuestion()
    {
        answerBox = new Dictionary<string, int>
        {
            { PlayerInfo.inputs[id][1], choices[0] },
            { PlayerInfo.inputs[id][2], choices[1] },
            { PlayerInfo.inputs[id][3], choices[2] },
        };

        for (int i = 0; i < choiceDisplays.Length; i++)
        {
            TMP_Text choice = choiceDisplays[i].transform.GetChild(0).GetComponent<TMP_Text>();

            choice.text = choices[i].ToString().ToUpper();
        }

        questionLabel.text = question.ToString();
    }

    /// <summary>
    /// This function handles checking the answer that the player inputted
    /// </summary>
    private void CheckAnswer()
    {
        string input = Input.inputString;
        CheckArrowKeyInputs();

        if (!answerBox.ContainsKey(input) || cooldown > 0f)
            return;

        if (!Input.GetKeyDown(input))
            return;

        if (answerBox[input] == question.answer)
        {
            Instantiate(sfxManager.sfxs[1], transform.position, Quaternion.identity, sfxManager.transform).Play();

            animator.StopAllCoroutines();
            StartCoroutine(animator.Jump());

            question.isAnswered = true;
            cooldown = 1f;
        }
        else
        {
            cooldown = PlayerInfo.cooldowns[PlayerInfo.chosenDifficulty[id]];
            Instantiate(sfxManager.sfxs[0], transform.position, Quaternion.identity, sfxManager.transform).Play();

            animator.StopAllCoroutines();
            StartCoroutine(animator.Fail());

            failCoroutine = FailureAnimation();
            StopCoroutine(failCoroutine);
            StartCoroutine(failCoroutine);
        }

        void CheckArrowKeyInputs()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                input = "left";
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                input = "down";
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                input = "right";
        }
    }

    /// <summary>
    /// Creates three different answers for the player to chose, one of them being the correct and the other two being incorrect
    /// </summary>
    private void ScrambleChoices()
    {
        choices = new int[3];
        int answerIndex = UnityEngine.Random.Range(0, 3);

        for (int i = 0; i < 3; i++)
        {
            if (i == answerIndex)
            {
                choices[i] = question.answer;
            }
            else
            {
                int rand = UnityEngine.Random.Range(-12, 12);

                while (rand == 0 || choices.ToList().Contains(question.answer + rand))
                    rand = UnityEngine.Random.Range(-6, 6);

                choices[i] = question.answer + rand;
            }
        }
    }

    /// <summary>
    /// Cooldown is constantly decreased while above 0
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cooldown()
    {
        while (true)
        {
            cooldown -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Hanldes the player movement to the next node
    /// </summary>
    /// <returns></returns>
    private IEnumerator Move()
    {
        Vector3 startPos = transform.position;
        Vector3 finalPos = nextNode.position - offset;
        float t = 0f;

        while (t <= 1f)
        {
            Array.ForEach(choiceDisplays, g => g.SetActive(false));
            questionLabel.gameObject.SetActive(false);

            transform.position = Vector3.Lerp(startPos, finalPos, Mathf.Sqrt(t));
            t += Time.deltaTime * 2f;

            yield return new WaitForEndOfFrame();
        }

        Array.ForEach(choiceDisplays, g => g.SetActive(true));
        questionLabel.gameObject.SetActive(true);

        cooldown = 0f;
    }

    /// <summary>
    /// Animation that plays when the player gets a question wrong
    /// </summary>
    /// <returns></returns>
    private IEnumerator FailureAnimation()
    {
        while (cooldown > 0f)
        {
            x.gameObject.SetActive(!x.gameObject.activeInHierarchy);

            yield return new WaitForSecondsRealtime(0.25f);
        }

        x.gameObject.SetActive(false);
    }
}
