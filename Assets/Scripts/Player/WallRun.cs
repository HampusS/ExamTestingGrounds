using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : BaseState
{
    public float timeBeforeFall = 1.6f;
    public float jumpStrengthMultiplier = 1.75f;
    float runTimeMultiplier = 2f;
    float timer = 0;
    bool initOnce;
    bool exit;

    Vector3 prevNormal, currNormal;
    TiltCamera camTilt;
    CameraControls camControl;

    private void Start()
    {
        myStateType = MoveStates.WALLRUN;
        camTilt = Camera.main.GetComponent<TiltCamera>();
        camControl = Camera.main.transform.parent.gameObject.GetComponent<CameraControls>();
    }

    void InitializeRun()
    {
        if (initOnce)
        {
            controller.onGravityMultiplier = false;
            rgdBody.useGravity = false;
            initOnce = false;
            exit = false;
            timer = 0;

            Vector3 result = (transform.forward + transform.up * 0.15f).normalized * controller.jumpStrength * jumpStrengthMultiplier;
            if (result.magnitude > rgdBody.velocity.magnitude)
                rgdBody.velocity = Vector3.ProjectOnPlane(result, controller.HorizontalHit().normal);
            else
            {
                rgdBody.velocity = Vector3.Project(rgdBody.velocity, result.normalized);
                rgdBody.velocity = Vector3.ProjectOnPlane(rgdBody.velocity, controller.HorizontalHit().normal);
            }
            camTilt.ResetCamera();
            if (controller.onLeftWall)
                camTilt.Right = true;
            else if (controller.onRightWall)
                camTilt.Left = true;
        }
    }

    public override bool Enter()
    {
        if (controller.onBottom)
            prevNormal = Vector3.zero;

        if (controller.onLeftWall || controller.onRightWall)
        {
            Debug.Log(Vector3.Dot(transform.forward, rgdBody.velocity.normalized));
            if (Input.GetButton("Jump") && Vector3.Dot(transform.forward, rgdBody.velocity.normalized) > 0)
            {
                Debug.Log("Sight");
                currNormal = controller.HorizontalHit().normal;
                if (currNormal != prevNormal)
                {
                    initOnce = true;
                    return true;
                }
            }
        }

        return false;
    }

    public override void Run()
    {
        InitializeRun();
        SupportRun();
        camControl.TurnToVector(new Vector3(rgdBody.velocity.x, 0, rgdBody.velocity.z));

        if (timer > 0 && Input.GetButtonDown("Jump"))
        {
            Vector3 result = (transform.forward + transform.up * 0.75f + controller.HorizontalHit().normal * 1.35f).normalized * controller.jumpStrength * jumpStrengthMultiplier;
            if (result.magnitude > rgdBody.velocity.magnitude)
                rgdBody.velocity = result.normalized * controller.jumpStrength;
            else
                rgdBody.velocity = Vector3.Project(rgdBody.velocity, result.normalized);

            exit = true;
        }

        if (controller.HorizontalHit().normal != currNormal)
            exit = true;

        if (Input.GetButton("Jump"))
            timer += Time.deltaTime;
        else
            timer += Time.deltaTime * runTimeMultiplier;

        if (timer >= timeBeforeFall * 0.5f)
            rgdBody.useGravity = true;

        if (!controller.onLeftWall && !controller.onRightWall || controller.onBottom && rgdBody.velocity.y < 0)
            exit = true;
    }

    public override bool Exit()
    {
        if (exit)
        {
            prevNormal = currNormal;
            camTilt.ResetCamera();
            return true;
        }
        return false;
    }

    void SupportRun()
    {
        if (!controller.onLeftWall && !controller.onRightWall)
            controller.UpdateRays();
    }

}
