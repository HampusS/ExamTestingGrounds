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
    public float speed = 50;
    RaycastHit hitAvoid;
    public bool rightAvoid { get; set; }
    public bool leftAvoid { get; set; }

    void Start()
    {
        myStateType = RocketState.AVIOD;
    }
    public override bool Enter()
    {
        if (controller.Rayhit())
        {
            if (controller.rightHitBool || controller.leftHitBool || controller.downHitBool || controller.upHitBool)
            {
                if (controller.Hit().collider != null && controller.Hit().collider.tag != "Player")
                {
                    Debug.Log("ender");
                    avoidVec = Vector3.ProjectOnPlane(controller.rb.velocity, controller.hit.normal).normalized;
                    controller.targetVect = avoidVec;
                    Debug.Log(controller.DownUpHit());
                   
                    return true;
                }
                else return false;
            }
            else return false;
        }
        else
            return false;
    }
    public override void Run()
    {
        rightAvoid = Physics.Raycast(transform.position, transform.right, out hitAvoid, 3);
        leftAvoid = Physics.Raycast(transform.position, -transform.right, out hitAvoid, 3);
        Debug.DrawRay(transform.position, transform.right * 1, Color.black);
        Debug.DrawRay(transform.position, -transform.right * 1, Color.black);
        if (controller.DownUpHit().collider != null && controller.DownUpHit().collider.tag != "Player")
        {
            Debug.Log("up");
            avoidVec = Vector3.ProjectOnPlane(controller.rb.velocity, controller.upDownHit.normal).normalized;
            controller.targetVect = avoidVec;
        }
    }
    public override bool Exit()
    {
        if (rightAvoid == false && leftAvoid == false)
        {
            return true;
        }
        else
            return false;
    }
}
