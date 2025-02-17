using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class SceneButton : MenuButton
{
    public int scene;
    [SerializeField] private Image transition;

    private void Awake()
    {
        transition = GameObject.FindGameObjectWithTag("Transition").GetComponent<Image>();
        transition.color = Color.clear;
    }

    protected override void Action()
    {
        base.Action();

        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && isSelected)
        {
            FindObjectOfType<PageNavigator>().currentlyNavigable = false;
            StartCoroutine(TransitionScene());
        }
    }

    /// <summary>
    /// Transitions to another scene with a fade in animation
    /// </summary>
    /// <returns></returns>
    private IEnumerator TransitionScene()
    {
        float t = 0f;

        AudioSource music = FindObjectsOfType<AudioSource>()
            .Single(audio => audio.transform.parent == null || audio.transform.parent.GetComponent<SoundManager>() == null);
        float initialVolume = music.volume;

        while (t < 2f)
        {
            transition.color = Color.Lerp(Color.clear, Color.black, (t / 2f));
            music.volume = initialVolume * (1 - (t / 2f));

            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        PlayerInfo.inGame = true;
        SceneManager.LoadScene(scene);
    }
}
