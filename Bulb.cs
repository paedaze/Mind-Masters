using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Bulb : MonoBehaviour
{
    public string key;
    public PlayerID id;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(key) && FindObjectsOfType<MemoryPlayer>().Single(p => p.id == id).canPlay)
        {
            StopAllCoroutines();
            StartCoroutine(ActivateBulb());
        }
    }

    /// <summary>
    /// Plays the animation of the bulb being lit
    /// </summary>
    /// <returns></returns>
    public IEnumerator ActivateBulb()
    {
        animator.SetBool("activated", false);

        yield return new WaitForEndOfFrame();

        animator.SetBool("activated", true);

        yield return new WaitForSecondsRealtime(0.25f);

        animator.SetBool("activated", false);
    }
}
