using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    public GameObject timer;
    public int resets = 1;
    public int strikes = 0;

    public void ResetSequence()
    {

        resets++;
        print(resets);

        int[] TargetCat = GameObject.Find("Target Cat").GetComponent<CatData>().data;
        int[] CustomCat = GameObject.Find("Custom Cat").GetComponent<CatData>().data;

        if (AreEqual(TargetCat, CustomCat))
        {
            ResetSuccess();
            Debug.Log("EQUAL");
        }
        else
        {
            ResetFaliure();
            Debug.Log("NOT EQUAL");
        }
    }


    public void ResetSuccess()
    {
        gameObject.BroadcastMessage("AnimateReset");
        gameObject.BroadcastMessage("AnimateResetSucess");
        timer.SendMessage("AdjustTime");
    }

    public void ResetFaliure()
    {
        gameObject.BroadcastMessage("AnimateReset");
        timer.SendMessage("ResetTime");
        strikes++;
    }

    bool AreEqual(int[] A, int[] B)
    {
        bool equal = true;
        for (int i = 0; i < A.Length; i++)
        {
            if (A[i] != B[i]) 
            { 
                equal = false; 
            }
        }
        return equal;
    }
}
