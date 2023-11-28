using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDisplay : MonoBehaviour
{
    public SpriteRenderer Sprite;

    public void Start()
    {
        Invoke("RefreshColorDisplay", 0.01f);
    }
    public void RefreshColorDisplay()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Sprite.color;
    }

    public void AnimateReset()
    {
        Invoke("RefreshColorDisplay", 0.5f);
    }
}
