using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrab : BaseState
{
    bool onLedge;
    bool onClimbUp;
    bool moveUp;
    bool moveIn;
    bool standUp;
    Vector3 targetPos;
    float animSpeed = 3;
    bool ForceExit;

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
            ForceExit = false;
        if (!ForceExit && ReachForLedge())
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
        if (onLedge && !ForceExit)
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
            ForceExit = true;
            onLedge = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            JumpFromWall((transform.forward + (controller.HorizontalHit().normal * 0.5f)).normalized, 150, 8);
            onLedge = false;
            ForceExit = true;
        }
    }

    public void ClimbUp()
    {
        Vector3 rayOrig = new Vector3(transform.localPosition.x - controller.HorizontalHit().normal.x, transform.position.y + 0.75f, transform.localPosition.z - controller.HorizontalHit().normal.z);
        Debug.DrawRay(rayOrig, Vector3.down, Color.red);
        if (onClimbUp)
        {
            if (!moveUp)
            {
                Vector3 ledgeOrig = new Vector3(transform.position.x, transform.position.y + 0.75f, transform.position.z);
                bool freeLedge = !Physics.Raycast(ledgeOrig, transform.forward, 1f);
                if (freeLedge)
                {

                    RaycastHit rayHit;
                    if (Physics.Raycast(rayOrig, Vector3.down, out rayHit, 0.5f))
                    {
                        targetPos = rayHit.point;
                        moveUp = true;
                        rgdBody.isKinematic = true;
                        GetComponent<CapsuleCollider>().enabled = false;
                    }
                    else
                        onClimbUp = false;
                }
                else
                    onClimbUp = false;
            }
            else if (moveUp)
            {
                if (!moveIn)
                {
                    Vector3 targetHeight = new Vector3(transform.position.x, targetPos.y, transform.position.z);
                    transform.position = Vector3.Lerp(transform.position, targetHeight, Time.deltaTime * animSpeed);

                    if (Vector3.Distance(transform.position, targetHeight) < 0.1f)
                    {
                        moveIn = true;
                    }
                }
                else if (!standUp)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * animSpeed);
                    if (Vector3.Distance(transform.position, targetPos) < 0.1f)
                    {
                        targetPos = new Vector3(transform.position.x, transform.position.y + 1.2f, transform.position.z);
                        standUp = true;
                    }
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * animSpeed);
                    if (Vector3.Distance(transform.position, targetPos) < 0.1f)
                    {
                        moveUp = false;
                        moveIn = false;
                        onLedge = false;
                        rgdBody.isKinematic = false;
                        GetComponent<CapsuleCollider>().enabled = true;
                        onClimbUp = false;
                        standUp = false;
                        ResetAllMovement();
                    }
                }
            }
        }
    }
}
