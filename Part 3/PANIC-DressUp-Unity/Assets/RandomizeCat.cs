using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeCat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RandomizeTargetCat();
    }

    public void RandomizeTargetCat()
    {
        gameObject.GetComponent<CatData>().RandomizeData();
    }
}
