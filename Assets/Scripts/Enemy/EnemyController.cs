using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour {

    public float speed = 5f;
    public Transform target;
    //public float rotateSpeed = 200;
    float maxspeed;
    float maxForce;
    private Rigidbody rb;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        rb.velocity = transform.forward * speed;
    }
    void FixedUpdate()
    {
        Vector3 dir = target.position - rb.position;

    }
    void UpdateController(Vector3 force)
    {
        //rb.velocity = force - rb.velocity;
    }
}
