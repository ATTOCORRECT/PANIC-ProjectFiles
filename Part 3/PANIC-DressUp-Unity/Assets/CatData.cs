using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatData : MonoBehaviour
{
    public int[] data;

    // DATA 0 Body Color
    public SpriteRenderer body;
    public Color[] bodyColor;
    
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
        body.color = bodyColor[data[0]];
    }

    public void ResetData()
    {
        data[0] = 0;
    }

    public void RandomizeData()
    {
        data[0] = Random.Range(0, bodyColor.Length);
    }

    public void CycleColorR()
    {
        data[0] = mod((data[0] + 1), bodyColor.Length);
        RefreshData();
    }

    public void CycleColorL()
    {
        data[0] = mod((data[0] - 1), bodyColor.Length);
        RefreshData();
    }

    int mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}
