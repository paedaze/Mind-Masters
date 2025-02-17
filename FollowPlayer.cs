using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform bg;
    [SerializeField] private Camera cam;
    private float lastYPos;

    private void Start()
    {
        foreach (Transform child in transform.parent)
        {
            if (child.GetComponent<Camera>() != null)
                cam = child.GetComponent<Camera>();
        }

        lastYPos = transform.position.y;
    }
    private void LateUpdate()
    {
        bg.position += Vector3.up * (transform.position.y - lastYPos);
        cam.transform.position += Vector3.up * (transform.position.y - lastYPos);

        lastYPos = transform.position.y;
    }
}
