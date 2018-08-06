using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimb : BaseState
{
    public float timeBeforeFall = 0.2f;
    float timer;
    bool initOnce;
    bool turning;
    bool exit;
    Vector3 prevNormal;
    CameraControls camControl;

    void Start()
    {
        myStateType = MoveStates.WALLCLIMB;
        camControl = Camera.main.transform.parent.gameObject.GetComponent<CameraControls>();
        snapStrength = 10;
    }

    private void InitializeRun()
    {
        if (initOnce)
        {
            controller.MultiplyGravity = true;
            rgdBody.useGravity = false;
            controller.ForceGravity = false;
            initOnce = false;
            turning = false;
            exit = false;
            timer = 0;
            rgdBody.velocity = Vector3.zero;
            rgdBody.AddForce(Vector3.up * controller.jumpHeight, ForceMode.VelocityChange);
            animator.SetTrigger("Sheath");
            animator.SetBool("WallClimb", true);
        }
    }

    public override bool Enter()
    {
        if (controller.onBottom)
            prevNormal = Vector3.zero;

        if (controller.onForwardWall && !inReachOfLedge() && Vector3.Dot(controller.HorizontalHit().normal, transform.forward) < -0.9f)
        {
            currNormal = controller.HorizontalHit().normal;            
            if (currNormal != prevNormal && Input.GetButton("Jump"))
            {
                initOnce = true;
                return true;
            }
        }

        return false;
    }

    public override void Run()
    {
        InitializeRun();
        if (!turning)
        {
            SnapToWall();
            if (timer >= timeBeforeFall)
                rgdBody.useGravity = true;

            if (timer > 0 && Input.GetButtonDown("Jump"))
            {
                controller.MultiplyGravity = false;
                rgdBody.useGravity = false;
                rgdBody.velocity = Vector3.zero;
                turning = true;
                camControl.LockTurning = true;
            }

            if (Input.GetButton("Jump"))
                timer += Time.deltaTime;
            else
                timer += Time.deltaTime * 2;

            if (controller.onBottom && rgdBody.velocity.y < 0)
                exit = true;

            if (inReachOfLedge())
                exit = true;
        }
        else if (TurnTowardsVector(controller.turnAroundSpeed, currNormal))
        {
            Vector3 result = (currNormal + transform.up).normalized;
            rgdBody.velocity = result * controller.jumpStrength;
            turning = false;
            exit = true;
        }

    }

    public override bool Exit()
    {
        if (exit)
        {
            camControl.LockTurning = false;
            prevNormal = currNormal;
            animator.SetBool("WallClimb", false);
            return true;
        }
        return false;
    }

    protected override void SnapToWall()
    {
        currPoint = controller.HorizontalHit().point;
        base.SnapToWall();
    }

}
