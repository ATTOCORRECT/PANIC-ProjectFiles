using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float time;
    public float maxTime;
    public float minTime;
    public GameObject root;
    public Reset resetScript;
    public TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        
        time = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
       
        time -= Time.deltaTime;
        text.text = Mathf.Round(time).ToString();

        if (time < 0)
        {
            root.SendMessage("ResetSequence");
        }
    }

    void AdjustTime()
    {
        maxTime = (maxTime/(resetScript.resets + 1)) + minTime;
        time = maxTime;
        
    }

    void ResetTime()
    {
        time = maxTime;
    }
}
