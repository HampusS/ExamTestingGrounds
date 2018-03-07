using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//requiers rigidbody
[RequireComponent(typeof(Rigidbody))]
public class HomingScript : MonoBehaviour
{

    public Transform target;
    public float speed;
    public float maxSpeed;
    public float rotateSpeed;
    private float dist;
    private Rigidbody rb;
    
    Vector3 dir;
    Vector3 desired;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {

    }
    void FixedUpdate()
    {
        dist = Vector3.Distance(rb.position, target.position) / 100;
        Debug.Log(dist);
        rotateSpeed = dist;

        dir = target.position - rb.position;
        dir.Normalize();
        rb.rotation = Quaternion.RotateTowards(transform.rotation, 
            Quaternion.LookRotation(dir), dist * Time.deltaTime);

        desired = dir * maxSpeed;
        Vector3 steering = -(desired - rb.velocity);

        steering = steering * rotateSpeed;

        rb.AddForce(-steering);
    }
}

