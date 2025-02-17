using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryAnimator : MonoBehaviour
{
    private Animator animator;
    private Vector3 origin;

    private void Start()
    {
        animator = GetComponent<Animator>();
        origin = transform.position;
    }

    private void Update()
    {
        if (animator.GetBool("fail") && GetComponent<SpriteRenderer>().sprite.name.Contains("fail"))
        {
            transform.position = origin + (Vector3.down * 0.1f);
        }
        else
        {
            transform.position = origin;
        }
    }

    /// <summary>
    /// Plays the animation of the player pressing the screen
    /// </summary>
    /// <returns></returns>
    public IEnumerator Press()
    {
        animator.SetBool("press", false);

        yield return new WaitForEndOfFrame();

        animator.SetBool("press", true);

        yield return new WaitForSecondsRealtime(0.15f);

        animator.SetBool("press", false);
    }

    /// <summary>
    /// Plays the fail animation of the player
    /// </summary>
    /// <returns></returns>
    public IEnumerator Fail()
    {
        animator.SetBool("fail", false);

        yield return new WaitForEndOfFrame();

        animator.SetBool("fail", true);

        yield return new WaitForSecondsRealtime(1f);

        animator.SetBool("fail", false);
    }

    /// <summary>
    /// Plays the victory animation of the player
    /// </summary>
    /// <returns></returns>
    public IEnumerator Win()
    {
        animator.SetBool("win", false);

        yield return new WaitForEndOfFrame();

        animator.SetBool("win", true);

        yield return new WaitForSecondsRealtime(1f);

        animator.SetBool("win", false);
    }
}
