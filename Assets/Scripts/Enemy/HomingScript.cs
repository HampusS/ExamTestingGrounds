using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//requiers rigidbody
[RequireComponent(typeof(Rigidbody))]
public class HomingScript : MonoBehaviour
{

    public Transform target;
    public float speed = 5f;
    public float maxSpeed = 4f;
    public float rotateSpeed = 0.01f;
    private Rigidbody rb;
    Vector3 acceleration = new Vector3(0,0,0);

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
        dir = target.position - rb.position;
        dir.Normalize();
        desired = dir * maxSpeed;
        //dir = maxSpeed*dir;
        //Vector3.ClampMagnitude(dir, maxSpeed);
        rb.velocity.Normalize();
        Vector3 steering = -(desired - rb.velocity);
        //Debug.Log("before"+steering);
        //steering.Normalize();
        //Vector3.ClampMagnitude(steering, rotateSpeed);
        //Debug.Log("after" + steering);
        //steering = steering * rotateSpeed;
        //acceleration = acceleration + steering;

        rb.AddForce(-steering);
    }
}
