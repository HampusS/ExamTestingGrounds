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
            if (controller.fwdHitBool)
            {
                if (controller.FwdRayHit().collider.tag == "Enviorment")
                {
                    return true;
                }
                return false;

            }
            else if (controller.rightHitBool)
            {
                if (controller.RightRayHit().collider.tag == "Enviorment")
                {
                    return true;
                }
                return false;

            }
            else if (controller.leftHitBool)
            {
                if (controller.LeftRayHit().collider.tag == "Enviorment")
                {
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
            Debug.Log("turn left");
        }
        else if (controller.leftHitBool)
        {
            avoidVec = controller.rightRayVector;
            Debug.Log("turn right");
        }
        avoidVec.Normalize();
        controller.steering = -avoidVec * maxSpeed;

        transform.rotation = Quaternion.Slerp(transform.rotation,
                                                     Quaternion.LookRotation(avoidVec),
                                                     avoidVec.magnitude* Time.deltaTime);
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
