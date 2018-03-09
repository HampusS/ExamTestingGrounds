using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimb : BaseState
{
    float timer;
    float timeSpan = 0.8f;
    float runHeight = 100;

    // Use this for initialization
    void Start()
    {
        myStateType = MoveStates.WALLCLIMB;
        timer = 0;
    }

    public override bool Enter()
    {
        if (controller.onForwardWall && controller.moveStates == MoveStates.GROUND)
        {
            if (Input.GetButtonDown("Jump") && Input.GetAxisRaw("Vertical") > 0)
            {
                GetComponent<Renderer>().material.color = Color.green;
                controller.Jump(transform.up * runHeight);
                controller.UpdateMoveAmount(0, 0);
                controller.EnableGravity(false);
                timer = 0;
                return true;
            }
        }

        return false;
    }

    public override void Run()
    {
        if (timer > 0 && Input.GetButtonDown("Jump"))
        {
            controller.Jump(controller.HorizontalHit().normal * 400);
            controller.Jump(transform.up * 200);
            timer += timeSpan;
        }

        if (Input.GetButton("Jump"))
            timer += Time.deltaTime;
        else
            timer += Time.deltaTime * 2;
    }

    public override bool Exit()
    {
        if (timer >= timeSpan)
        {
            GetComponent<Renderer>().material.color = Color.red;
            controller.EnableGravity(true);
            timer = 0;
            return true;
        }
        return false;
    }

}
