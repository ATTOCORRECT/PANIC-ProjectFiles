using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatGenerator : MonoBehaviour
{
    public int CatAmount;
    public GameObject CatPrefab;
    public GameObject[] CatList;
    public Color32[] colors;

    public int searchFurColor;
    public int searchSplotchColor;
    
    // Start is called before the first frame update
    void Start()
    {
        searchFurColor = Random.Range(0, colors.Length);
        searchSplotchColor = Random.Range(0, colors.Length);
        
        CatList = new GameObject[CatAmount];

        
        for (int i = 0; i < CatAmount; i++)
        {

            CatList[i] = Instantiate(CatPrefab);

            if (i == 0) //dedicated target cat
            {
                CatPrefab.GetComponent<Cat>().IsTarget(true);

            }
            else
            {
                CatPrefab.GetComponent<Cat>().IsTarget(false);
            }

        }
    }

    void IsTargetCat(int[] parameters)
    {
        print("searching for " + searchFurColor + " and " + searchSplotchColor);

        if (parameters[0] == searchFurColor && parameters[1] == searchSplotchColor)
        {
            print("found");

        }

        else
        {
            print("incorrect");
        }
    }

   
}
