using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    IDEL,
    ENDPHASE,
    AVIOD,
    CHASE,
}

[RequireComponent(typeof(Rigidbody))]
public class MissileController : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float maxSpeed;
    public float rotateSpeed;
    public float rayAngle;

    private float dist;
    private Rigidbody rb;

    Vector3 dir;
    Vector3 desired;
    Vector3 right;
    Vector3 left;
    public float rayLenght = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        UpdateRays();
    }

    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        dir = target.position - rb.position;
        dir.Normalize();
        desired = dir * maxSpeed;
        Vector3 steering = -(desired - rb.velocity);

        steering = steering * rotateSpeed;
        transform.rotation = Quaternion.Slerp(transform.rotation,
                                                     Quaternion.LookRotation(dir),
                                                     dir.magnitude * Time.deltaTime);
        rb.AddForce(-steering);
    }
    void UpdateRays()
    {
        Ray ray = new Ray(transform.position, Vector3.forward);
        right = transform.forward - transform.right * (-rayAngle);
        left = transform.forward - transform.right * (rayAngle);

        Debug.DrawRay(ray.origin, transform.forward * rayLenght, Color.blue);
        Debug.DrawRay(ray.origin, right * rayLenght, Color.red);
        Debug.DrawRay(ray.origin, left * rayLenght, Color.green);
    }
}
