using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public List<Transform> points;
    public float speed;

    private int srcPoint;
    private int dstPoint;
    private float currentPos;  // от 0 до 1
    private float dir;

    void Start()
    {
        srcPoint = 0;
        dstPoint = 1;
        currentPos = 0.0f;
        dir = 1.0f;
    }

    void Update()
    {
        if (points.Count < 2)
            return;

        var inPos = Move(points[srcPoint], points[dstPoint]);
        if (inPos)
        {
            currentPos = 0.0f;
            if (dstPoint == 0 || dstPoint == points.Count - 1)
            {
                dir = -dir;
                var temp = dstPoint;
                dstPoint = srcPoint;
                srcPoint = temp;
            }
            else
            {
                var inc = dir > 0 ? 1 : -1;
                dstPoint += inc;
                srcPoint += inc;
            }
        }
    }

    private bool Move(Transform src, Transform dst)
    {
        currentPos += speed * Time.deltaTime;
        transform.position = Vector3.Lerp(src.position, dst.position, currentPos);

        return currentPos > 1.0f;
    }
}
