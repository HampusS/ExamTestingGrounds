using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrab : BaseState
{
    bool onLedge;
    bool onClimbUp;
    Vector3 targetPos;


    void Start()
    {
        myStateType = MoveStates.LEDGEGRAB;
    }

    void Initialize()
    {
        onLedge = true;
        onClimbUp = false;
        EnableGravity(false);
        controller.onGravityMultiplier = false;
        ResetAllMovement();
    }

    public override bool Enter()
    {
        if (ReachForLedge())
        {
            Debug.Log("Enter");
            Initialize();
            return true;
        }
        return false;
    }

    public override void Run()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.75f, transform.position.z);
        bool freeLedge = !Physics.Raycast(position, transform.forward, 1f);
        if (controller.HorizontalHit().collider != null && freeLedge)
            onLedge = false;
        if (!onClimbUp)
        {
        }
        Debug.Log(rgdBody.velocity);
        Debug.Log(controller.FinalMove);

        CheckForInput();
        ClimbUp();
    }

    public override bool Exit()
    {
        if (onLedge)
            return false;
        Debug.Log("Exit");
        EnableGravity(true);
        controller.onGravityMultiplier = true;
        return true;
    }

    public void CheckForClimb()
    {

    }

    public void CheckForInput()
    {
        float forward = Input.GetAxisRaw("Vertical");

        if (forward > 0)
            onClimbUp = true;
        else if (forward < 0)
            onLedge = false;

        if (Input.GetButtonDown("Jump"))
        {
            JumpFromWall((transform.forward + (controller.HorizontalHit().normal * 0.5f)).normalized, 150, 8);
            onLedge = false;
            Debug.Log("Jump");
        }
    }

    public void ClimbUp()
    {
        if (onClimbUp)
        {
            // Move player to position on top of ledge
        }
    }
}
