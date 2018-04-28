using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrab : BaseState
{
    bool onTryClimbUp;
    bool onLedge;
    bool onMoveUp;
    public float climbSpeed = 4;

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
        rgdBody.useGravity = false;
        controller.onGravityMultiplier = false;
        rgdBody.velocity = Vector3.zero;
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
    }

    public override bool Exit()
    {
        if (onLedge)
            return false;
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

        if (Input.GetButtonDown("Jump") && !controller.onForwardWall)
        {
            Vector3 result = (transform.up + transform.forward).normalized;
            rgdBody.velocity = result * controller.jumpStrength;
            onLedge = false;
        }
        if (!controller.onLeftWall && !controller.onRightWall)
            controller.UpdateRays();
        if (!controller.onForwardWall && !controller.onLeftWall && !controller.onRightWall)
            onLedge = false;
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
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * climbSpeed);

                if (Vector3.Distance(transform.position, targetPos) < 0.15f)
                {
                    onMoveUp = false;
                    onLedge = false;
                    onTryClimbUp = false;
                    rgdBody.isKinematic = false;
                    GetComponent<CapsuleCollider>().enabled = true;
                }

            }
        }
    }
}
