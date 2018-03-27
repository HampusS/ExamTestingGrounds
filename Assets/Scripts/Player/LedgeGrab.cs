using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrab : BaseState
{
    bool onForceExit;
    bool onClimbUp;
    bool onLedge;
    bool onMoveUp;
    float animSpeed = 2;
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
        if (controller.onBottom)
            onForceExit = false;
        if (!onForceExit && ReachForLedge())
        {
            Initialize();
            return true;
        }
        return false;
    }

    public override void Run()
    {
        if (!onClimbUp)
        {
            //if (rgdBody.velocity.y > 0)
            //    onLedge = false;
            CheckForInput();
        }
        else
            ClimbUp();
    }

    public override bool Exit()
    {
        if (onLedge && !onForceExit)
            return false;

        EnableGravity(true);
        controller.onGravityMultiplier = true;
        return true;
    }

    public void CheckForInput()
    {
        float forward = Input.GetAxisRaw("Vertical");

        if (forward > 0)
            onClimbUp = true;
        else if (forward < 0)
        {
            onForceExit = true;
            onLedge = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            JumpFromWall((transform.forward + (controller.HorizontalHit().normal * 0.5f)).normalized, 150, 8);
            onLedge = false;
            onForceExit = true;
        }
    }

    public void ClimbUp()
    {

        if (onClimbUp)
        {
            if (!onMoveUp)
            {
                Vector3 ledgeOrig = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                Debug.DrawRay(ledgeOrig, transform.forward, Color.red);
                bool freeLedge = !Physics.Raycast(ledgeOrig, transform.forward, 1f);
                if (freeLedge)
                {
                    Vector3 rayOrig = new Vector3(transform.localPosition.x - (controller.HorizontalHit().normal.x * 1.75f), transform.position.y + 1.55f, transform.localPosition.z - (controller.HorizontalHit().normal.z * 1.75f));
                    Debug.DrawRay(rayOrig, Vector3.down, Color.red);
                    targetPos = rayOrig;
                    onMoveUp = true;
                    rgdBody.isKinematic = true;
                    GetComponent<CapsuleCollider>().enabled = false;
                }
                else
                    onClimbUp = false;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * animSpeed);

                if (Vector3.Distance(transform.position, targetPos) < 0.1f)
                {
                    onMoveUp = false;
                    onLedge = false;
                    onClimbUp = false;
                    rgdBody.isKinematic = false;
                    GetComponent<CapsuleCollider>().enabled = true;
                    ResetAllMovement();
                }

            }
        }
    }
}
