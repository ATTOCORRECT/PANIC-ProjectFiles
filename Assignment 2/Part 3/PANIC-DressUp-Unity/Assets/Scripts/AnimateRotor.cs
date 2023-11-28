using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateRotor : MonoBehaviour
{
    public void AnimateReset()
    {
        Debug.Log("animating R");
        StartCoroutine(SequenceRotate());
    }

    IEnumerator SequenceRotate()
    {
        yield return new WaitForSeconds(0.5f);

        float seconds = 1f;
        float steps = seconds * 50;

        for (int i = 0; i < steps; i++)
        {
            transform.rotation = Quaternion.Euler(0,0, transform.rotation.eulerAngles.z - 8);

            yield return new WaitForSeconds(0.02f);
        }
    }
}
