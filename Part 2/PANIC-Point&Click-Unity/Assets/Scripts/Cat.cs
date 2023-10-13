using UnityEngine;

public class Cat : MonoBehaviour
{
    // Start is called before the first frame update
    int[] parameters;
    public GameObject CatHandler;
    public CatGenerator CatGenerator;
    public Color32[] CatColors;
    
    private SpriteRenderer furRender;
    private SpriteRenderer splotchRender;

    public bool isTarget;

     void Start()
    {
        CatHandler = GameObject.Find("CatHandler");
        CatGenerator = FindObjectOfType<CatGenerator>();
        CatColors = FindObjectOfType<CatGenerator>().colors;

        furRender = GetComponent<SpriteRenderer>();
        splotchRender = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

        parameters = new int[2];

            if (isTarget == true) //target cat generates with the target colors
            {
                parameters[0] = CatGenerator.searchFurColor;
                parameters[1] = CatGenerator.searchSplotchColor;
            }
            else 
            {
            parameters[0] = Random.Range(0, CatColors.Length); //all cat fur colors
            parameters[1] = Random.Range(0, CatColors.Length); //all cat splotch colors

                while (parameters[0] == CatGenerator.searchFurColor && parameters[1] == CatGenerator.searchSplotchColor) 
                {
                    parameters[0] = Random.Range(0, CatColors.Length); //all cat fur colors
                    parameters[1] = Random.Range(0, CatColors.Length); //all cat splotch colors
                print("dupe found");
                }
            }

        furRender.color = CatColors[parameters[0]]; //change fur color
        splotchRender.color = CatColors[parameters[1]]; //change splotch color
    }

    public void IsTarget(bool state)
    {
        isTarget = state;
    }

    private void OnMouseDown()
    {
        CatHandler.SendMessage("IsTargetCat", parameters);
    }
}
