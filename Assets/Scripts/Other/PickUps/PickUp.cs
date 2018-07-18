using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform target;
    protected float dragRange = 6;
    protected float dragSpeed = 15;
    // Use this for initialization
    void Start()
    {

    }

    public void DragToPlayer(Transform target)
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
            DragToPlayer(target);
    }
}
