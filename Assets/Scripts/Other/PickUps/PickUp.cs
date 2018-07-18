using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform target;
    public float dragRange = 6;
    public float dragSpeed = 25;

    public void DragToTarget(Transform target)
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance < dragRange)
        {
            float inverse = 1 / distance;
            //Debug.Log(inverse + " " + inverse * dragSpeed);

            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * inverse * inverse * inverse * dragSpeed);
        }
    }

    private void Update()
    {
        if (target != null)
            DragToTarget(target);
    }
}
