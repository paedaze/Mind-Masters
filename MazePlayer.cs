using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class MazePlayer : MonoBehaviour
{
    private MazeObjects objs;
    private SoundManager sfxManager;

    #region Gameplay 

    private Maze maze;
    private PlayerID id;
    private PlayerDifficulty diff;
    private bool canPlay;
    private Camera cam;

    #endregion

    #region UI

    [SerializeField] private TMP_Text difficultyLabel;
    [SerializeField] private TMP_Text x;
    private Vector3 inputBarOffset = new Vector3(615f, -2.5f, -5400f);

    #endregion

    #region Input Bar

    [SerializeField] private GameObject inputBar;
    [SerializeField] private GameObject[] inputObjects;
    [SerializeField] private float inputListStartOffset;
    private List<GameObject> playerInputs;

    #endregion

    private void Start()
    {
        sfxManager = FindObjectOfType<SoundManager>();
        objs = FindObjectOfType<MazeObjects>();

        cam = transform.parent.GetComponentInChildren<Camera>();
        canPlay = true;
        id = (PlayerID) int.Parse(transform.parent.name);
        diff = PlayerInfo.chosenDifficulty[id];

        playerInputs = new List<GameObject>();

        SetDifficultyLabel();
        CreateMaze();
    }

    private void Update()
    {
        SetInputBar();

        if (FindObjectOfType<MinigameManager>().isPlayable && canPlay)
        {
            string input = Input.inputString;
            CheckArrowInputs();

            if (PlayerInfo.inputs[id].Contains(input) && Input.GetKeyDown(input))
            {
                Guess(input);
            }

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
    }

    // UI Functions
    /// <summary>
    /// Sets the difficulty label to the player's corresponding difficulty
    /// </summary>
    private void SetDifficultyLabel()
    {
        difficultyLabel.text = Enum.GetName(typeof(PlayerDifficulty), diff);
        difficultyLabel.color = PlayerInfo.difficultyColors[diff];
    }

    /// <summary>
    /// Scales the position of the input bar to anchor to the right side of the camera view
    /// </summary>
    private void SetInputBar()
    {
        Camera cam = transform.parent.GetComponentInChildren<Camera>();
        float camWidth = cam.aspect * cam.orthographicSize;

        RectTransform rt = inputBar.GetComponent<RectTransform>();

        // Already switches between world space and camera space in different script
        if (camWidth > 11f)
        {
            rt.transform.localPosition = inputBarOffset;

            bool isTwoPlayer = PlayerInfo.playerCount == 2;
            bool threePlayerAndIsThird = PlayerInfo.playerCount == 3 && id == PlayerID.Player3;

            if (isTwoPlayer || threePlayerAndIsThird)
            {
                rt.transform.localPosition = inputBarOffset + (Vector3.right * 20f);
            }
        }
        else
        {
            rt.anchoredPosition = new Vector2(-50f, 0f);
        }
    }

    // Maze generation functions
    /// <summary>
    /// Creates and sets a new maze
    /// </summary>
    private void CreateMaze()
    {
        maze = new Maze(diff, transform.position);
        for (int y = 0; y < maze.nodes.Length; y++)
        {
            for (int x = 0; x < maze.nodes[y].Length; x++)
            {
                Node node = maze.nodes[y][x];
                if (node.isTraversable)
                {
                    InstantiateRoadObject(node.dir, new Vector2Int(x, y));
                }
                else if (UnityEngine.Random.Range(0, 10) >= 8)
                {
                    GameObject foliage = objs.foliage[UnityEngine.Random.Range(0, objs.foliage.Length)];
                    Instantiate(foliage, node.position, Quaternion.identity);
                }

                GameObject rand = objs.nonTraversables[UnityEngine.Random.Range(0, objs.nonTraversables.Length)];
                Instantiate(rand, node.position, Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// Instantiates a road object and handles logic on deciding which direction the road should be heading in
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="gridPos"></param>
    private void InstantiateRoadObject(Direction dir, Vector2Int gridPos)
    {
        // Generates a straight path for the first and last road nodes
        if (gridPos.y == 0 || gridPos.y - 1 == maze.nodes.Length)
        {
            Instantiate(objs.traversables[2], maze.nodes[gridPos.y][gridPos.x].position, Quaternion.identity);
            return;
        }

        bool hasInstantiated = false;
        Node previousNode = maze.nodes[gridPos.y - 1][gridPos.x];
        Node currentNode = maze.nodes[gridPos.y][gridPos.x];

        switch (dir)
        {
            case (Direction.Left):
                HorizontalTransition();
                break;
            case (Direction.Right):
                HorizontalTransition();
                break;
            case (Direction.Up):
                VerticalTransition();
                break;
        }

        StraightPath();


        void HorizontalTransition()
        {
            UpdatePreviousAndCurrentNode();

            if (maze.GetLowerNode(currentNode.gridPosition).dir == Direction.Up)
            {
                if (dir == Direction.Left)
                    Instantiate(objs.traversables[0], currentNode.position, Quaternion.identity);
                else
                    Instantiate(objs.traversables[1], currentNode.position, Quaternion.identity);

                hasInstantiated = true;
            }
        }

        void VerticalTransition()
        {
            UpdatePreviousAndCurrentNode();

            if (previousNode.isTraversable && maze.nodes[currentNode.gridPosition.y].Where(node => (int)node.dir >= 1).Count() == 0)
                return;

            if (maze.nodes[gridPos.y].Where(node => node.dir == Direction.Left).Count() > 0)
            {
                Instantiate(objs.traversables[3], currentNode.position, Quaternion.identity);
                hasInstantiated = true;
            }
            else
            {
                Instantiate(objs.traversables[4], currentNode.position, Quaternion.identity);
                hasInstantiated = true;
            }
        }

        void StraightPath()
        {
            UpdatePreviousAndCurrentNode();

            if (hasInstantiated) return;

            GameObject road = Instantiate(objs.traversables[2], currentNode.position, Quaternion.identity);

            if (dir == Direction.Left || dir == Direction.Right)
            {
                road.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
            }
        }

        void UpdatePreviousAndCurrentNode()
        {
            Node previousNode = maze.nodes[gridPos.y - 1][gridPos.x];
            Node currentNode = maze.nodes[gridPos.y][gridPos.x];
        }
    }

    // Gameplay functions
    /// <summary>
    /// Processes the user's guess and determines whether it is correct within the sequence
    /// </summary>
    /// <param name="input"></param>
    private void Guess(string input)
    {
        int index = 0;
        for (int i = 0; i < PlayerInfo.inputs[id].Length; i++)
        {
            if (PlayerInfo.inputs[id][i] == input && i != 2)
            {
                index = i;
                break;
            }

            // Returns if UP, RIGHT, or LEFT inputs are not detected
            if (i == PlayerInfo.inputs[id].Length - 1) return;
        }

        // Instantiates arrow direction on the right side of the screen (indicates which directions the player has chosen) and adds it to a list
        if (playerInputs.Count == 0)
            playerInputs.Add(Instantiate(inputObjects[index], inputBar.transform.position + (inputListStartOffset * Vector3.up), Quaternion.identity, inputBar.transform));
        else
            playerInputs.Add(Instantiate(inputObjects[index], playerInputs[playerInputs.Count - 1].transform.position + (0.7f * Vector3.down), Quaternion.identity, inputBar.transform));

        // Converts the index of the player input into a 'Direction' and checks to see if it aligns with the sequence of the maze
        if ((Direction)index != maze.sequence[playerInputs.Count - 1])
        {
            Instantiate(sfxManager.sfxs[0], transform.position, Quaternion.identity, sfxManager.transform).Play();
            StartCoroutine(FailAnimation());
        }
        else if (maze.sequence.Count == playerInputs.Count)
        {
            Instantiate(sfxManager.sfxs[1], transform.position, Quaternion.identity, sfxManager.transform).Play();
            StartCoroutine(SucessAnimation());
        }
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

        ClearInputList();
        canPlay = true;
    }

    /// <summary>
    /// Plays when the player sucessfully completes a sequence
    /// </summary>
    /// <returns></returns>
    private IEnumerator SucessAnimation()
    {
        canPlay = false;
        PlayerInfo.scores[id] += PlayerInfo.difficultyScale[diff];

        float speed = 15f;
        float t = 0f;
        Vector3 origin = transform.position;

        // Lerp through the road on success
        for (int i = 1; i < maze.nodeSequence.Count; i++)
        {
            t = 0f;
            origin = transform.position;
            Node node = maze.nodeSequence[i];

            while (t <= 1)
            {
                t = Mathf.Clamp01(t);
                transform.position = Vector2.Lerp(origin, node.position, t);

                float x = node.position.x - transform.position.x;
                float y = node.position.y - transform.position.y;
                float angle = (Mathf.Atan2(y, x) * Mathf.Rad2Deg) - 90f;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                t += Time.deltaTime * speed;
                yield return new WaitForEndOfFrame();
            }
        }

        // Lerps to the first node of the next maze
        t = 0f;
        origin = transform.position;

        while (t <= 1f)
        {
            t = Mathf.Clamp01(t);
            transform.position = Vector2.Lerp(origin, origin + (Vector3.up * 1.008f), t);

            t += Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }

        ClearInputList();
        CreateMaze();
        StartCoroutine(MoveScreens());
    }

    /// <summary>
    /// Moves to the next maze
    /// </summary>
    private IEnumerator MoveScreens()
    {
        float t = 0f;
        Vector3 origin = cam.transform.position;

        int centerX = (int)Mathf.Ceil(PlayerInfo.width / 2f);
        int leftCenterY = (PlayerInfo.length / 2) - 1;
        int rightCenterY = (PlayerInfo.length / 2);

        float y = maze.nodes[rightCenterY][centerX].position.y - (maze.nodes[rightCenterY][centerX].position.y - maze.nodes[leftCenterY][centerX].position.y);
        Vector3 target = new Vector3(transform.position.x, y, -10f);

        while (t < 1f)
        {
            cam.transform.position = Vector3.Lerp(origin, target, t);

            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        canPlay = true;
    }

    /// <summary>
    /// Clears the list of inputs
    /// </summary>
    private void ClearInputList()
    {
        for (int i = 0; i < playerInputs.Count; i++)
        {
            Destroy(playerInputs[i].gameObject);
            playerInputs.RemoveAt(i);
            i--;
        }
    }
}
