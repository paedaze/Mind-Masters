using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioSource> sfxs;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<AudioSource>() != null)
            {
                sfxs.Add(child.GetComponent<AudioSource>());
            }
        }
    }
}
