using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : BaseState
{
    float timer, timeSpan;
    bool running;

    float runHeight = 60;
    bool onWall;
    RaycastHit myHit;

    private void Start()
    {
        myStateType = MoveStates.WALLRUN;
        timer = 0;
        timeSpan = 1.2f;
    }

    void InitializeRun()
    {
        GetComponent<Renderer>().material.color = Color.green;
        myHit = controller.HorizontalHit();
        running = true;
        timer = 0;
    }

    public override bool Enter()
    {
        if (Input.GetButtonDown("Jump") && Input.GetAxisRaw("Vertical") > 0)
        {
            if (controller.onLeftWall || controller.onRightWall || controller.onForwardWall)
            {
                InitializeRun();

                if (controller.onForwardWall)
                {
                    runHeight = 100;
                    timeSpan = 0.8f;
                    controller.UpdateMoveAmount(0, 0);
                }
                else
                {
                    runHeight = 50;
                    timeSpan = 1f;
                    controller.SetMoveAmount(Vector3.ProjectOnPlane(controller.finalMove, myHit.normal));
                }

                return true;
            }
        }
        return false;
    }

    public override void Run()
    {
        if (timer > 0 && Input.GetButtonDown("Jump"))
        {
            Debug.Log(myHit.normal);
            rgdBody.AddForce(myHit.normal * 300);
            rgdBody.AddForce(transform.up * 150);
            timer = timeSpan;
        }

        if (Input.GetButton("Jump"))
            timer += Time.deltaTime;
        else
            timer += Time.deltaTime * 2;


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
        if (timer >= timeSpan)
        {
            GetComponent<Renderer>().material.color = Color.red;
            rgdBody.useGravity = true;
            timer = 0;
            return true;
        }

        return false;
    }

}
