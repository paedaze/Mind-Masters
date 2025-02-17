using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class CameraSetup : MonoBehaviour
{
    #region Camera Scaling

    [SerializeField] private Vector3 offset;
    // Camera positions for each amount of players
    private Dictionary<int, Vector2[]> rectPositions = new Dictionary<int, Vector2[]>
    {
        {2, PlayerInfo.twoPlayerSetup },
        {3, PlayerInfo.threePlayerSetup },
        {4, PlayerInfo.fourPlayerSetup }
    };
    // Dimensions for each camera according to the number of players
    private Dictionary<int, Vector2> dimensions = new Dictionary<int, Vector2>
    {
        {2, new Vector2(1f, 0.5f) },
        {3, new Vector2(0.5f, 0.5f) },
        {4, new Vector2(0.5f, 0.5f) }
    };

    #endregion

    #region Text Scaling

    // All the cameras in the scene
    public Camera[] cameras;
    private Vector3 textOffset = new Vector3(-660f, 300f, -5400f);

    #endregion

    #region Borders

    [SerializeField] private GameObject[] borders;

    #endregion

    private void Start()
    {
        GameObject[] players = FindObjectOfType<PlayerManager>().players;
        cameras = players
            .Select(player => player.GetComponentInChildren<Camera>())
            .Where(cam => cam.gameObject.activeInHierarchy)
            .ToArray();

        SetCameraDimension();
        SetBorders();
    }

    private void Update()
    {
        ScaleIDToScreen();
    }


    /// <summary>
    /// Sets the cameras' dimensions and placement in the view
    /// </summary>
    /// <param name="count"></param>
    private void SetCameraDimension()
    {
        int count = PlayerInfo.playerCount;

        for (int i = 0; i < cameras.Length; i++)
        {
            float offset = (count == 3 && i == 2) ? 0.25f : 0f;
            cameras[i].rect = new Rect(rectPositions[count][i].x, rectPositions[count][i].y, dimensions[count].x + offset, dimensions[count].y);
        }
    }

    /// <summary>
    /// Changes in the player ID label's position relative to the screen's aspect ratio
    /// </summary>
    private void ScaleIDToScreen()
    {
        for (int i = 1; i <= cameras.Length; i++)
        {
            string id = i.ToString();
            TMP_Text playerIDText = FindObjectsOfType<TMP_Text>().Single(tmpText => tmpText.text.Contains(id) && tmpText.text.Contains("P"));

            float camWidth = cameras[i - 1].aspect * cameras[i - 1].orthographicSize;
            Canvas canvas = cameras[i - 1].GetComponentInChildren<Canvas>();

            if (camWidth > 11f)
            {
                canvas.renderMode = RenderMode.WorldSpace;
                playerIDText.transform.localPosition = textOffset;

                bool isTwoPlayer = PlayerInfo.playerCount == 2;
                bool threePlayerAndIsThird = PlayerInfo.playerCount == 3 && i == 3;

                if (isTwoPlayer || threePlayerAndIsThird)
                {
                    playerIDText.transform.localPosition = textOffset + (Vector3.left * 15f);
                }
            }
            else
            {
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                playerIDText.rectTransform.anchoredPosition = Vector2.zero;
            }

            // Ensures canvas remains at a constant scale and position when changing render modes
            if (canvas.GetComponent<RectTransform>() != null)
                canvas.GetComponent<RectTransform>().localScale = Vector3.one * (1f / 60f);

            canvas.transform.localPosition = Vector3.forward * 100f;
        }
    }

    /// <summary>
    /// Splits the screen with a black border depending on the amount of players
    /// </summary>
    private void SetBorders()
    {
        switch (PlayerInfo.playerCount)
        {
            case 2:
                borders[1].SetActive(true);
                break;
            case 3:
                borders[0].SetActive(true);
                borders[1].SetActive(true);
                break;
            case 4:
                borders[0].SetActive(true);
                borders[1].SetActive(true);
                borders[2].SetActive(true);
                break;
        }
    }
}
