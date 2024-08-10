using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 EndPosition;
    public float Duration;
    public void Move()
    {
        StartCoroutine(MoveToPosition());
    }

    IEnumerator MoveToPosition()
    {
        float elapsedTime = 0;
        Vector3 start = targetTransform.position;
        while (elapsedTime < Duration)
        {
             targetTransform.position = Vector3.Lerp(start, EndPosition, elapsedTime/ Duration);
             elapsedTime += Time.deltaTime;
             yield return null;

        }
        transform.position = EndPosition;
       
    }
}