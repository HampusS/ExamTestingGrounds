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
        if (controller.rightHitBool || controller.leftHitBool || controller.downHitBool || controller.upHitBool)
        {
            if (controller.Hit().collider != null || controller.DownUpHit().collider != null)
            {
                Debug.Log("enter avoid");
                //Debug.Log("wall dot:"+Vector3.Dot(controller.targetVect, Vector3.ProjectOnPlane(controller.targetVect, controller.hit.normal).normalized));
                if (Vector3.Dot(controller.targetVect, Vector3.ProjectOnPlane(controller.targetVect, controller.hit.normal).normalized) > 0.95f)
                {                
                    if (controller.DownUpHit().collider != null && controller.DownUpHit().collider.tag != "Player")
                    {
                        avoidVec = Vector3.ProjectOnPlane(controller.rb.velocity, controller.upDownHit.normal).normalized;
                        controller.targetVect = avoidVec;
                        //Debug.Log("up dot:" + Vector3.Dot(controller.targetVect, Vector3.ProjectOnPlane(controller.targetVect, controller.hit.normal).normalized));

                        return true;
                    }
                }
                if (controller.Hit().collider != null && controller.Hit().collider.tag != "Player")
                {
                    avoidVec = Vector3.ProjectOnPlane(controller.rb.velocity, controller.hit.normal).normalized;
                    controller.targetVect = avoidVec;
                }
                return true;
            }
            else return false;
        }
        else return false;
    }
    public override void Run()
    {
        rightAvoid = Physics.Raycast(transform.position, transform.right, out hitAvoid, 3);
        leftAvoid = Physics.Raycast(transform.position, -transform.right, out hitAvoid, 3);
        Debug.DrawRay(transform.position, transform.right * 1, Color.black);
        Debug.DrawRay(transform.position, -transform.right * 1, Color.black);

    }
    public override bool Exit()
    {
        if (rightAvoid == false && leftAvoid == false || controller.downHitBool == true)
        {
            Debug.Log("exit avoid   ");
            return true;
        }
        else
            return false;
    }
}
