using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseState : MonoBehaviour
{
    public MoveStates myStateType { get; set; }
    protected PlayerController controller;
    protected Rigidbody rgdBody;
    protected Vector3 currNormal;
    protected Vector3 currPoint;
    protected float snapStrength = 1;
    protected Animator animator;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        rgdBody = GetComponent<Rigidbody>();
        myStateType = MoveStates.ERROR;
        animator = GetComponentInChildren<Animator>();
    }

    public abstract bool Enter();
    public abstract void Run();
    public abstract bool Exit();

    protected bool TurnTowardsVector(float rotateSpeed, Vector3 target)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target), Time.deltaTime * rotateSpeed);

        if (Vector3.Dot(transform.forward, target) < 0.999f)
            return false;
        return true;       
    }

    public bool inReachOfLedge()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.85f, transform.position.z);

        if (controller.onForwardWall && !Physics.Raycast(position, transform.forward, 1))
            return true;
        return false;
    }
    
    protected virtual void SnapToWall()
    {
        Vector3 snapPos = currPoint + (currNormal * controller.capsule.radius);
        transform.position = Vector3.Lerp(transform.position, snapPos, Time.deltaTime * snapStrength);
    }

}
