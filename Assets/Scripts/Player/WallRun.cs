using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : BaseState
{
    [SerializeField]
    LayerMask wallLayer;
    RaycastHit hit;
    float timer, timeSpan;
    bool running;

    // Use this for initialization
    void Start()
    {
        timer = 0;
        timeSpan = 0.8f;
    }

    public override bool Enter()
    {
        if (controller.Grounded && Input.GetButtonDown("Jump") && Input.GetAxisRaw("Vertical") > 0)
        {
            if (Physics.Raycast(transform.position, transform.right, out hit, 1, wallLayer) ||
                Physics.Raycast(transform.position, -transform.right, out hit, 1, wallLayer))
            {
                GetComponent<Renderer>().material.color = Color.green;
                timer = 0;
                running = true;
                controller.Grounded = false;
                return true;
            }
        }
        return false;
    }

    public override void Run()
    {
        timer += Time.deltaTime;
        if (running)
        {
            rgdBody.useGravity = false;
            transform.position += Vector3.up * (GetComponent<CapsuleCollider>().height * 0.15f);
            rgdBody.velocity = new Vector3(rgdBody.velocity.x, 0, rgdBody.velocity.z);
            rgdBody.AddForce(transform.up * 60);
            running = false;
        }
    }

    public override bool Exit()
    {
        if (timer >= timeSpan || !Input.GetButton("Jump"))
        {
            GetComponent<Renderer>().material.color = Color.red;
            rgdBody.useGravity = true;
            timer = 0;
            return true;
        }

        return false;
    }

}
