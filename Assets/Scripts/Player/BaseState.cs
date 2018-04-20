using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseState : MonoBehaviour
{
    public MoveStates myStateType { get; set; }
    protected PlayerController controller;
    protected Rigidbody rgdBody;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        rgdBody = GetComponent<Rigidbody>();
        myStateType = MoveStates.ERROR;
    }

    public abstract bool Enter();
    public abstract void Run();
    public abstract bool Exit();

    protected bool TurnTowardsVector(float rotateSpeed, Vector3 targetVector)
    {
        float step = rotateSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetVector, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
        
        if (Vector3.Dot(transform.forward, targetVector) < 0.999f)
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
    
}
