using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Counters : MonoBehaviour
{
    public TextMeshProUGUI Success;
    public TextMeshProUGUI Fail;
    int successes = 0;
    int faliures = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseSuccesses()
    {
        successes += 1;
        Success.text = successes + "";
    }

    public void IncreaseFaliures()
    {
        faliures += 1;
        Fail.text = faliures + "/10";
    }
}
