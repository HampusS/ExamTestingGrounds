using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrab : BaseState
{
    bool onTryClimbUp;
    bool onLedge;
    bool onMoveUp;
    bool init = true;
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
        init = false;
    }

    public override bool Enter()
    {
        if (inReachOfLedge())
        {
            init = true;
            return true;
        }
        return false;
    }

    public override void Run()
    {
        if(init)
            Initialize();
        CheckForInput();
        if (onTryClimbUp)
            ClimbUp();
    }

    public override bool Exit()
    {
        if (onLedge)
            return false;
        ResetPlayer();
        return true;
    }

    void ResetPlayer()
    {
        onMoveUp = false;
        onLedge = false;
        onTryClimbUp = false;
        rgdBody.isKinematic = false;
        GetComponent<CapsuleCollider>().enabled = true;
    }

    public void CheckForInput()
    {
        float forward = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("Jump") || forward > 0)
        {
            onTryClimbUp = true;
            posToDown = new Vector3(transform.localPosition.x - (controller.HorizontalHit().normal.x * 1.25f), transform.position.y + 1, transform.localPosition.z - (controller.HorizontalHit().normal.z * 1.25f));
            posToForward = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        }
        else if (Input.GetButtonDown("Jump") && !controller.onForwardWall)
        {
            Vector3 result = (transform.up + transform.forward).normalized;
            rgdBody.velocity = result * controller.jumpStrength;
            onLedge = false;
        }

        if (forward < 0 || !controller.onForwardWall && !onTryClimbUp)
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
                    onLedge = false;
            }
        }
    }
}
