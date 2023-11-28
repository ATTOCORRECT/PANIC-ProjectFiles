using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSprites : MonoBehaviour
{
    [System.NonSerialized]
    public SpriteRenderer furRender;
    [System.NonSerialized]
    public SpriteRenderer splotchRender;

    void Start()
    {
        furRender = transform.GetChild(0).GetComponent<SpriteRenderer>();
        splotchRender = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        furRender.sortingOrder = (int)(-transform.position.y * 1000);
        splotchRender.sortingOrder = (int)(-transform.position.y * 1000) + 1;
    }
}
