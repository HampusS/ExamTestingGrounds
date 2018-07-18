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
    bool wasOnLedge;
    Vector3 targetPos;
    Vector3 posToGround;
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
        controller.MultiplyGravity = false;
        rgdBody.velocity = Vector3.zero;
        init = false;
        wasOnLedge = true;
        currNormal = controller.forwardHit.normal;
        currPoint = controller.forwardHit.point;
    }

    public override bool Enter()
    {
        if (controller.onBottom)
            wasOnLedge = false;
        if (inReachOfLedge() && !wasOnLedge)
        {
            init = true;
            return true;
        }
        return false;
    }

    public override void Run()
    {
        if (init)
            Initialize();
        CheckForInput();
        SnapToWall();
        if (onTryClimbUp)
            ClimbUp();
        Debug.DrawRay(posToGround, Vector3.down, Color.red);
        Debug.DrawRay(posToForward, transform.forward, Color.green);
        Debug.DrawRay(posToGround, Vector3.up * 1.5f, Color.red);
        snapStrength = 10;
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

        if (Input.GetButtonDown("Jump") && controller.onForwardWall || forward > 0 && controller.onForwardWall)
        {
            onTryClimbUp = true;
            posToGround = new Vector3(transform.localPosition.x - (controller.forwardHit.normal.x * 0.7f), transform.position.y + 0.75f, transform.localPosition.z - (controller.forwardHit.normal.z * 0.7f));
            posToForward = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        }
        else if (Input.GetButtonDown("Jump") && !controller.onForwardWall)
        {
            Vector3 result = (transform.up + transform.forward).normalized;
            rgdBody.velocity = result * controller.jumpStrength;
            onLedge = false;
            wasOnLedge = false;
        }

        if (forward < 0)
            onLedge = false;

    }

    protected override void SnapToWall()
    {
        if (onLedge && wasOnLedge && !onMoveUp)
        {
            base.SnapToWall();
        }
    }

    public void ClimbUp()
    {
        if (onTryClimbUp)
        {
            if (!onMoveUp)
            {
                RaycastHit groundHit;
                bool freeInfront = !Physics.Raycast(posToForward, transform.forward, 1f);
                bool freeAbove = !Physics.Raycast(posToGround, Vector3.up, 1.5f);
                bool onToGround = Physics.Raycast(posToGround, Vector3.down, out groundHit, 1f);

                if (freeInfront && freeAbove && onToGround)
                {
                    targetPos = new Vector3(posToGround.x, groundHit.point.y + 1.15f, posToGround.z);
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
