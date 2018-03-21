using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//requiers rigidbody
//[RequireComponent(typeof(Rigidbody))]
public class HomingScript : MonoBehaviour
{

    //public Transform target;
    //public float speed;
    //public float maxSpeed;
    //public float rotateSpeed;
    //private float dist;
    //private Rigidbody rb;
    
    //Vector3 dir;
    //Vector3 desired;

    //void Start()
    //{
    //    rb =
    //    //rb = GetComponent<Rigidbody>();
    //}
    //void Update()
    //{

    //}
    //void FixedUpdate()
    //{  
    //    dir = target.position - rb.position;
    //    dir.Normalize();
    //    desired = dir * maxSpeed;
    //    Vector3 steering = -(desired - rb.velocity);

    //    steering = steering * rotateSpeed;
    //    transform.rotation = Quaternion.Slerp(transform.rotation,
    //                                                 Quaternion.LookRotation(dir),
    //                                                 dir.magnitude * Time.deltaTime);

    //    Vector3 fwd = transform.TransformDirection(Vector3.forward);
    //    Debug.DrawRay(transform.position, fwd, Color.green,10);
    //    if (Physics.Raycast(transform.position, fwd, 10))
    //        print("There is something in front of the object!");

    //    rb.AddForce(-steering);
    //}
}

