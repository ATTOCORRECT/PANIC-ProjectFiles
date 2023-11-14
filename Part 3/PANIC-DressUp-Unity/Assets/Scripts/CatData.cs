using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatData : MonoBehaviour
{
    public int[] data;

    // DATA 0 Body Color
    public SpriteRenderer body;
    public Color[] bodyColor;

    // DATA 1 Pattern Color
    public SpriteRenderer pattern;
    public Color[] patternColor;

    // DATA 2 Top Sprite
    public SpriteRenderer top;
    public Sprite[] topSprite;

    // DATA 3 Middle Sprite
    public SpriteRenderer middle;
    public Sprite[] middleSprite;

    // DATA 4 Bottom Sprite
    public SpriteRenderer bottom;
    public Sprite[] bottomSprite;

    // Start is called before the first frame update
    void Start()
    {
        RefreshData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RefreshData()
    {
        body.color    = bodyColor[   data[0]];
        pattern.color = patternColor[data[1]];
        top.sprite    = topSprite[   data[2]];
        middle.sprite = middleSprite[data[3]];
        bottom.sprite = bottomSprite[data[4]];
    }

    public void ResetData()
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = 0;
        }
        RefreshData();
    }

    public void RandomizeData()
    {
        data[0] = Random.Range(0, bodyColor.   Length);
        data[1] = Random.Range(0, patternColor.Length);
        data[2] = Random.Range(0, topSprite.   Length);
        data[3] = Random.Range(0, middleSprite.Length);
        data[4] = Random.Range(0, bottomSprite.Length);
        RefreshData();
    }

    public void CycleBodyColorR() // Body Color
    {
        data[0] = mod((data[0] + 1), bodyColor.Length);
        RefreshData();
    }

    public void CycleBodyColorL()
    {
        data[0] = mod((data[0] - 1), bodyColor.Length);
        RefreshData();
    }
    public void CyclePatternColorR() // Pattern Color
    {
        data[1] = mod((data[1] + 1), patternColor.Length);
        RefreshData();
    }

    public void CyclePatternColorL()
    {
        data[1] = mod((data[1] - 1), patternColor.Length);
        RefreshData();
    }

    public void CycleTopSpriteR() // Top Sprite
    {
        data[2] = mod((data[2] + 1), topSprite.Length);
        RefreshData();
    }

    public void CycleTopSpriteL()
    {
        data[2] = mod((data[2] - 1), topSprite.Length);
        RefreshData();
    }
    public void CycleMiddleSpriteR() // Middle Sprite
    {
        data[3] = mod((data[3] + 1), middleSprite.Length);
        RefreshData();
    }

    public void CycleMiddleSpriteL()
    {
        data[3] = mod((data[3] - 1), middleSprite.Length);
        RefreshData();
    }

    public void CycleBottomSpriteR() // Bottom Sprite
    {
        data[4] = mod((data[4] + 1), bottomSprite.Length);
        RefreshData();
    }

    public void CycleBottomSpriteL()
    {
        data[4] = mod((data[4] - 1), bottomSprite.Length);
        RefreshData();
    }

    int mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}
