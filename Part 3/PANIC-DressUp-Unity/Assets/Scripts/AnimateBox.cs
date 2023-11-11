using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateBox : MonoBehaviour
{
    public void AnimateResetSucess()
    {
        Debug.Log("animating B");
        StartCoroutine(SequenceMoveRight());
    }

    IEnumerator SequenceMoveRight()
    {
        yield return new WaitForSeconds(0.15f);

        transform.position = new Vector2(0, 0);

        yield return new WaitForSeconds(0.35f);

        float seconds = 1f;
        float steps = seconds * 50;

        for (int i = 0; i < steps; i++)
        {
            transform.position = Vector2.Lerp(new Vector2(0, 0), new Vector2(12, 0), (i + 1) / steps);

            yield return new WaitForSeconds(0.02f);
        }
    }
}