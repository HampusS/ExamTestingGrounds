using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorControls : MonoBehaviour
{    
    public Transform StartPos;
    public Transform TargetPos;
    public bool Activated;
    [SerializeField]
    GameObject openFence;
    [SerializeField]
    GameObject closeFence;

    float speed = 15;

    Rigidbody player;

    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.SetParent(transform);
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.transform.parent = null;
    }

    // Use this for initialization
    void Start()
    {
        transform.position = StartPos.position;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Activated)
        {
            closeFence.SetActive(true);
            if (Input.GetButtonDown("Jump"))
            {
                Vector3 relativeVel = (TargetPos.position - transform.position).normalized;
                //if (Activated/* && relativeVel.y > 0*/)
                    player.velocity += relativeVel * speed;
            }
            float distance = Vector3.Distance(transform.position, TargetPos.position);
            transform.position = Vector3.MoveTowards(transform.position, TargetPos.position, Time.deltaTime * speed);

            if (distance < 0.15f)
            {
                transform.position = TargetPos.position;
                Transform temp = StartPos;
                StartPos = TargetPos;
                TargetPos = temp;
                Activated = false;
                openFence.SetActive(false);
            }
        }
    }
}
