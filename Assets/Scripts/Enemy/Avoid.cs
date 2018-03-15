using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avoid : BaseEnemyState
{

    public float maxSpeed;
    public float rotateSpeed;
    Rigidbody rbg;

    Vector3 dir;
    Vector3 desired;
    Vector3 avoidVec;
    void Start()
    {
        myStateType = RocketState.AVIOD;
    }
    public override bool Enter()
    {
        if (controller.Rayhit())
        {
            Debug.Log("enter if avoid");
            if (controller.fwdHitBool)
            {
                if (controller.FwdRayHit().collider.tag == "Enviorment")
                {
                    Debug.Log("enterAvoid");
                    return true;
                }
                return false;

            }
            else if (controller.rightHitBool)
            {
                if (controller.RightRayHit().collider.tag == "Enviorment")
                {
                    Debug.Log("enterAvoid");
                    return true;
                }
                return false;

            }
            else if (controller.leftHitBool)
            {
                if (controller.LeftRayHit().collider.tag == "Enviorment")
                {
                    Debug.Log("enterAvoidLeft");
                    return true;
                }
                return false;
            }
            else
                return false;
        }
        else
            return false;
    }
    public override void Run()
    {
        if (controller.rightHitBool)
        {
            avoidVec = controller.leftRayVector;
        }
        else if (controller.leftHitBool)
        {
            avoidVec = controller.rightRayVector;
            Debug.Log("turn right");
        }

        //Vector3 tar = controller.target.position;
        dir = controller.target.position - controller.rb.position;
        dir.Normalize();
        desired = dir * maxSpeed;
        controller.steering = -(desired - controller.rb.velocity);// + avoidVec;

        controller.steering = (controller.steering + -(avoidVec)) - controller.rb.position;
        controller.steering = controller.steering * rotateSpeed;

        transform.rotation = Quaternion.Slerp(transform.rotation,
                                                     Quaternion.LookRotation(dir),
                                                     dir.magnitude * Time.deltaTime);
    }
    public override bool Exit()
    {
        if (!controller.Rayhit())
        {
            return true;
        }
        else
            return false;
    }
}
