using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatGenerator : MonoBehaviour
{
    public int CatAmount;
    public GameObject CatPrefab;
    public GameObject[] CatList;
    public Color32[] colors;
    public Sprite[] CatPatterns;

    public int searchFurColor;
    public int searchSplotchColor;
    public int searchSplotchPattern;

    bool found = false;

    // Start is called before the first frame update
    void Start()
    {
        var availableColors = new List<int> { 0, 1, 2, 3, 4 };

        int randomFurColor = Random.Range(0, colors.Length);
        searchFurColor = availableColors[randomFurColor];
        availableColors.Remove(randomFurColor);

        int randomSplotchColor = Random.Range(0, colors.Length - 1);
        searchSplotchColor = availableColors[randomSplotchColor];
        availableColors.Remove(randomSplotchColor);

        searchSplotchPattern = Random.Range(0, 5);

        print("searching for " + searchFurColor + " and " + searchSplotchColor + " with " + searchSplotchPattern);

        CatList = new GameObject[CatAmount];

        
        for (int i = 0; i < CatAmount; i++)
        {
            float y = Random.Range(-5, 5);
            float x = Random.Range(-5 / 9f * 16f, 5 / 9f * 16f);
            Vector2 randomPoint = new Vector2(x, y);
            Debug.Log(randomPoint);
            CatList[i] = Instantiate(CatPrefab, randomPoint, Quaternion.identity);

            if (i == 0) //dedicated target cat
            {
                CatPrefab.GetComponent<Cat>().IsTarget(true);

            }
            else
            {
                CatPrefab.GetComponent<Cat>().IsTarget(false);
            }

        }

        GameObject LookUpCat = Instantiate(CatPrefab,new Vector2(-7,4), Quaternion.identity);
        LookUpCat.GetComponent<CatMovement>().enabled = false;
        LookUpCat.GetComponent<OrderSprites>().enabled = false;
        LookUpCat.GetComponent<Cat>().enabled = false;
        LookUpCat.GetComponent<CircleCollider2D>().enabled = false;
        LookUpCat.transform.GetChild(0).GetComponent<SpriteRenderer>().color = colors[searchFurColor];
        LookUpCat.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 10010;
        LookUpCat.transform.GetChild(1).GetComponent<SpriteRenderer>().color = colors[searchSplotchColor];
        LookUpCat.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = CatPatterns[searchSplotchPattern];
        LookUpCat.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 10011;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (found)
            {
                print("found");
                Invoke("RestartScene", 1f);
            }
            else
            {
                print("Notfound");
            }
        }
    }

    void IsTargetCat()
    {
        found = true;
    }

    void RestartScene()
    {
        SceneManager.LoadScene("GameScene");
    }
   
}
