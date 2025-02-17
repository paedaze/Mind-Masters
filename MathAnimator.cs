using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathAnimator : MonoBehaviour
{
    private Animator animator;
    private Vector3 origin;
    private MathPlayer mp;

    private void Start()
    {
        mp = GetComponentInParent<MathPlayer>();
        origin = transform.position;
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        OffsetPlayer();
    }

    /// <summary>
    /// Offsets player during certain animations
    /// </summary>
    private void OffsetPlayer()
    {
        if (animator.GetBool("fail") && GetComponent<SpriteRenderer>().sprite.name.Contains("fail"))
        {
            transform.position = origin + (Vector3.down * 0.1f);
        }
        else if (transform.position == origin + (Vector3.down * 0.1f))
        {
            transform.position = origin;
        }
        else
        {
            origin = transform.position;
        }
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

        yield return new WaitForSecondsRealtime(PlayerInfo.cooldowns[PlayerInfo.chosenDifficulty[mp.id]] - 0.1f);

        animator.SetBool("fail", false);
    }

    /// <summary>
    /// Plays the jump animation of the player
    /// </summary>
    /// <returns></returns>
    public IEnumerator Jump()
    {
        animator.SetBool("win", false);

        yield return new WaitForEndOfFrame();

        animator.SetBool("win", true);

        yield return new WaitForSecondsRealtime(0.6f);

        animator.SetBool("win", false);
    }
}
