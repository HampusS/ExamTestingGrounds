using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrab : BaseState
{
    bool onTryClimbUp;
    bool onLedge;
    bool onMoveUp;
    float animSpeed = 2;
    Vector3 targetPos;

    Vector3 posToDown;
    Vector3 posToForward;

    void Start()
    {
        myStateType = MoveStates.LEDGEGRAB;
    }

    void Initialize()
    {
        onLedge = true;
        onTryClimbUp = false;
        EnableGravity(false);
        controller.onGravityMultiplier = false;
        ResetAllMovement();
    }

    public override bool Enter()
    {
        if (inReachOfLedge())
        {
            Initialize();
            return true;
        }
        return false;
    }

    public override void Run()
    {
        if (!onTryClimbUp)
            CheckForInput();
        else
            ClimbUp();
        Debug.DrawRay(posToDown, Vector3.down, Color.red);
        Debug.DrawRay(posToDown, Vector3.up, Color.black);
        Debug.DrawRay(posToForward, transform.forward, Color.red);
    }

    public override bool Exit()
    {
        if (onLedge)
            return false;

        EnableGravity(true);
        controller.onGravityMultiplier = true;
        return true;
    }

    public void CheckForInput()
    {
        float forward = Input.GetAxisRaw("Vertical");

        if (forward > 0 && controller.onForwardWall)
        {
            onTryClimbUp = true;
            posToDown = new Vector3(transform.localPosition.x - (controller.HorizontalHit().normal.x * 1.25f), transform.position.y + 1, transform.localPosition.z - (controller.HorizontalHit().normal.z * 1.25f));
            posToForward = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        }
        else if (forward < 0 || forward > 0 && !controller.onForwardWall)
            onLedge = false;

        if (Input.GetButtonDown("Jump") && !inReachOfLedge())
        {
            JumpFromWall((transform.forward + (controller.HorizontalHit().normal * 0.5f)).normalized, 150, 8);
            onLedge = false;
        }
    }

    public void ClimbUp()
    {
        if (onTryClimbUp)
        {
            if (!onMoveUp)
            {
                RaycastHit groundHit;
                bool freeLedge = !Physics.Raycast(posToForward, transform.forward, 1f);
                bool onToGround = Physics.Raycast(posToDown, Vector3.down, out groundHit, 1f);

                if (freeLedge && onToGround)
                {
                    targetPos = new Vector3(posToDown.x, groundHit.point.y + 1.15f, posToDown.z);
                    onMoveUp = true;
                    rgdBody.isKinematic = true;
                    GetComponent<CapsuleCollider>().enabled = false;
                }
                else
                    onTryClimbUp = false;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * animSpeed);

                if (Vector3.Distance(transform.position, targetPos) < 0.15f)
                {
                    onMoveUp = false;
                    onLedge = false;
                    onTryClimbUp = false;
                    rgdBody.isKinematic = false;
                    GetComponent<CapsuleCollider>().enabled = true;
                    ResetAllMovement();
                }

            }
        }
    }
}
