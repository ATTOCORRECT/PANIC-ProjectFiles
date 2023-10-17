using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    // Start is called before the first frame update
    int[] parameters;
    public GameObject CatHandler;
    public CatGenerator CatGenerator;
    public Color32[] CatColors;
    public Sprite[] CatPatterns;

    private SpriteRenderer furRender;
    private SpriteRenderer splotchRender;

    public bool isTarget = false;

     void Start()
    {
        CatHandler = GameObject.Find("CatHandler");
        CatGenerator = FindObjectOfType<CatGenerator>();
        CatColors = FindObjectOfType<CatGenerator>().colors;
        CatPatterns = FindObjectOfType<CatGenerator>().CatPatterns;

        furRender = transform.GetChild(0).GetComponent<SpriteRenderer>();
        splotchRender = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();

        parameters = new int[3];

            if (isTarget == true) //target cat generates with the target colors
            {
                parameters[0] = CatGenerator.searchFurColor;
                parameters[1] = CatGenerator.searchSplotchColor;
                parameters[2] = CatGenerator.searchSplotchPattern;
        }
            else 
            {
                GenerateParameters();

            while (parameters[0] == CatGenerator.searchFurColor && parameters[1] == CatGenerator.searchSplotchColor && parameters[2] == CatGenerator.searchSplotchPattern)
            {
                GenerateParameters();

                print("dupe found");
                }
            }

        furRender.color = CatColors[parameters[0]]; //change fur color
        splotchRender.color = CatColors[parameters[1]]; //change splotch color
        splotchRender.sprite = CatPatterns[parameters[2]]; //change splotch pattern

        gameObject.GetComponent<CircleCollider2D>().enabled = isTarget;
    }

    private void Update()
    {
        if (isTarget)
        {
            Vector2 position = transform.position;
            Debug.DrawLine(position + new Vector2 (1, 1), position + new Vector2(-1, -1));
            Debug.DrawLine(position + new Vector2(1, -1), position + new Vector2(-1, 1));
        }
    }

    public void IsTarget(bool state)
    {
        isTarget = state;
    }

    private void OnMouseDown()
    {
        CatHandler.SendMessage("IsTargetCat");
    }

    void GenerateParameters()
    {
        var availableColors = new List<int> { 0, 1, 2, 3, 4 };

        int randomFurColor = Random.Range(0, CatColors.Length);
        parameters[0] = availableColors[randomFurColor]; //all cat fur colors
        availableColors.Remove(randomFurColor);

        int randomSplotchColor = Random.Range(0, CatColors.Length - 1);
        parameters[1] = availableColors[randomSplotchColor]; //all cat splotch colors
        availableColors.Remove(randomSplotchColor);

        parameters[2] = Random.Range(0, CatColors.Length); //all cat splotch patterns
    }
}
