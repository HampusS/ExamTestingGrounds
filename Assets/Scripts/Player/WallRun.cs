using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : BaseState
{
    [SerializeField]
    LayerMask wallLayer;
    float timer, timeSpan;
    bool running;
    //tested with different values, was 30 when i started
    float runHeight = 60;
    bool onWall, forward;

    private void Start()
    {
        myStateType = MoveStates.WALLRUN;
        timer = 0;
        timeSpan = 1.2f;
    }

    void TraceForWalls()
    {
        onWall = false;
        if (Physics.Raycast(transform.position, transform.right, controller.rayLengthHorizontal, wallLayer) ||
                Physics.Raycast(transform.position, -transform.right, controller.rayLengthHorizontal, wallLayer) ||
                Physics.Raycast(transform.position, transform.forward, controller.rayLengthHorizontal, wallLayer))
        {
            forward = Physics.Raycast(transform.position, transform.forward, controller.rayLengthHorizontal, wallLayer);
            onWall = true;
        }
    }

    public override bool Enter()
    {
        if (controller.moveStates == MoveStates.GROUND && Input.GetButtonDown("Jump") && Input.GetAxisRaw("Vertical") > 0)
        {
            TraceForWalls();
            if (onWall)
            {
                if (forward)
                {
                    runHeight = 100;
                    timeSpan = 0.8f;
                    controller.UpdateMoveAmount(0, 0);
                }
                else
                {
                    runHeight = 50;
                    timeSpan = 1f;
                }
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
