using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : BaseState
{
    [SerializeField]
    LayerMask wallLayer;
    float timer, timeSpan;
    bool running;
    float runHeight = 60;
    bool onWall;

    private void Start()
    {
        myStateType = MoveStates.WALLRUN;
        timer = 0;
        timeSpan = 0.8f;
    }

    void TraceForWalls()
    {
        onWall = false;
        if (Physics.Raycast(transform.position, transform.right, 1, wallLayer) ||
                Physics.Raycast(transform.position, -transform.right, 1, wallLayer))
            onWall = true;
    }

    public override bool Enter()
    {
        if (controller.moveStates == MoveStates.GROUND && Input.GetButtonDown("Jump") && Input.GetAxisRaw("Vertical") > 0)
        {
            TraceForWalls();
            if (onWall)
            {
                GetComponent<Renderer>().material.color = Color.green;
                controller.moveStates = myStateType;
                running = true;
                timer = 0;
                return true;
            }
        }
        return false;
    }

    public override void Run()
    {
        timer += Time.deltaTime;
        TraceForWalls();

        if (running)
        {
            rgdBody.useGravity = false;
            transform.position += Vector3.up * (GetComponent<CapsuleCollider>().height * 0.15f);
            rgdBody.velocity = new Vector3(rgdBody.velocity.x, 0, rgdBody.velocity.z);
            rgdBody.AddForce(transform.up * runHeight);
            running = false;
        }
    }

    public override bool Exit()
    {
        if (timer >= timeSpan || !Input.GetButton("Jump") || !onWall)
        {
            GetComponent<Renderer>().material.color = Color.red;
            rgdBody.useGravity = true;
            timer = 0;
            return true;
        }

        return false;
    }

}
