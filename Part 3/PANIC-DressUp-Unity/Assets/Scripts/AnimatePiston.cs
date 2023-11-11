using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatePiston : MonoBehaviour
{
    public void AnimateReset()
    {
        Debug.Log("animating P");
        StartCoroutine(SequenceMoveDown());
        StartCoroutine(SequenceMoveUp());
    }

    IEnumerator SequenceMoveDown()
    {
        float seconds = 0.1f;
        float steps = seconds * 50;

        for (int i = 0; i < steps; i++)
        {
            transform.position = Vector2.Lerp(new Vector2(0, 9), new Vector2(0, 3), (i + 1) / steps);

            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator SequenceMoveUp()
    {
        yield return new WaitForSeconds(0.2f);

        float seconds = 0.2f;
        float steps = seconds * 50;

        for (int i = 0; i < steps; i++)
        {
            transform.position = Vector2.Lerp(new Vector2(0, 3), new Vector2(0, 9), (i + 1) / steps);

            yield return new WaitForSeconds(0.02f);
        }
    }
}
