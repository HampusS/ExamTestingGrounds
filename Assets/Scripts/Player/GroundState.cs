using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : BaseState
{

    private void Start()
    {
        myStateType = MoveStates.GROUND;
    }

    public override bool Enter()
    {
        if (controller.onBottom)
            return true;
        return false;
    }

    public override void Run()
    {
        Vector3 direction = transform.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"))).normalized;
        rgdBody.AddForce(direction * controller.moveSpeed, ForceMode.Acceleration);
        rgdBody.velocity = Vector3.ClampMagnitude(rgdBody.velocity, controller.maxSpeed);

        if (Input.GetButtonDown("Jump"))
            rgdBody.AddForce(transform.up * controller.jumpHeight, ForceMode.Impulse);

    }

    public override bool Exit()
    {
        return true;
    }

}
