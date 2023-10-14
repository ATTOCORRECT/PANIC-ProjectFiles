using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CatMovement : MonoBehaviour
{
    Vector3 xp;
    Vector3 y, yd;
    float k1, k2, k3;
    public float averageTimeInterval;
    public float range;
    public float f, z, r;
    Vector2 targetPosition = Vector2.zero;
    bool run = true;
    void Start()
    {
        Vector3 x0 = targetPosition;
        //compute constants

        // initialize variables
        xp = x0;
        y = x0;
        yd = Vector3.zero;
    }

    void FixedUpdate()
    {
        if (run)
        {
            run = false;
            float randomTime = averageTimeInterval + Random.Range(-averageTimeInterval / 2, averageTimeInterval / 2);
            StartCoroutine(moveTarget(randomTime));
        }

        Debug.DrawLine(Vector2.zero, targetPosition);

        // second order dynamics vv
        k1 = z / (Mathf.PI * f);
        k2 = 1 / ((2 * Mathf.PI * f) * (2 * Mathf.PI * f));
        k3 = r * z / (2 * Mathf.PI * f);


        float T = Time.fixedDeltaTime;
        Vector3 x = targetPosition;
        Vector3 xd = (x - xp) / T;
        xp = x;

        float k2Stable = Mathf.Max(k2, T * T / 2 + T * k1 / 2, T*k1);
        y = y + T * yd;
        yd = yd + T * (x + k3 * xd - y - k1 * yd) / k2;

        if (Vector3.Magnitude(y - x) < 0.01)
        {
            y = x;
        }

        transform.position = y;
    }

    IEnumerator moveTarget(float time)
    {
        Vector2 currentPoint = targetPosition;
        Vector2 randomPoint = new Vector2(Random.Range(-range / 9 * 16, range / 9 * 16), Random.Range(-range, range));

        float loopCount = 20 * time;
        for (int i = 0; i < loopCount; i++)
        {
            targetPosition = Vector2.Lerp(currentPoint, randomPoint, (float)i / loopCount);
            
            yield return new WaitForSeconds(0.05f);
        }
        run = true;
    }
}
