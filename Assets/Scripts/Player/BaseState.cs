using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseState : MonoBehaviour
{
    public MoveStates myStateType { get; set; }
    protected PlayerController controller;
    protected Rigidbody rgdBody;
    protected bool onLedge;

    protected Vector3 moveAmount;
    protected Vector3 targetMove;
    Vector3 smoothMove;
    Vector3 currVect;
    float maxSpeed = 10;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        rgdBody = GetComponent<Rigidbody>();
        myStateType = MoveStates.ERROR;
    }

    public abstract bool Enter();
    public abstract void Run();
    public abstract bool Exit();

    protected void Jump(float magnitude)
    {
        rgdBody.velocity = Vector3.zero;
        rgdBody.AddForce(transform.up * magnitude);
    }

    protected bool TurnTowardsVector(float rotateSpeed, Vector3 targetVector)
    {
        float step = rotateSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetVector, step, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);

        return newDir == targetVector;
    }

    protected void ResetAllMovement()
    {
        rgdBody.angularVelocity = Vector3.zero;
        rgdBody.velocity = Vector3.zero;
        SetMoveAmount(Vector3.zero);
    }

    protected void EnableGravity(bool enable)
    {
        rgdBody.useGravity = enable;
    }

    public bool ReachForLedge()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.75f, transform.position.z);
        Debug.DrawRay(position, transform.forward * 1, Color.green);

        if (controller.onForwardWall && !Physics.Raycast(position, transform.forward, 1))
        {
            return true;
        }
        return false;
    }

    protected void JumpFromWall(float height, float perpendicularStrength)
    {
        AppendFinalMove(controller.HorizontalHit().normal * perpendicularStrength);
        Jump(height);
        //moveAmount = Vector3.Project(-moveAmount, controller.HorizontalHit().normal);
    }

    protected void JumpFromWall(Vector3 direction, float height, float perpendicularStrength)
    {
        SetMoveAmount(direction * perpendicularStrength);
        Jump(height);
        //moveAmount = Vector3.Project(-moveAmount, controller.HorizontalHit().normal);
    }

    // ----------------------------------------------------------------------------------------------------------------

    protected void UpdateMoveAmount(float moveFloatiness, Vector3 targetMove)
    {
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMove, ref smoothMove, moveFloatiness);
        controller.FinalMove = transform.TransformDirection(moveAmount);
    }

    protected void AppendFinalMove(Vector3 move)
    {
        controller.FinalMove += move;
        controller.FinalMove = Vector3.ClampMagnitude(controller.FinalMove, maxSpeed);
    }

    protected void SetMoveAmount(Vector3 newVect)
    {
        moveAmount = newVect;
        controller.FinalMove = newVect;
    }

    // ----------------------------------------------------------------------------------------------------------------
    // ---------------------------------------------TESTING------------------------------------------------------------

    protected void UpdateMoveInput(float speed)
    {
        Vector3 strafe = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        targetMove = strafe * speed;
    }

    protected Vector3 TransformVector(Vector3 inVect)
    {
        return transform.TransformDirection(inVect);
    }

    protected Vector3 ProjectVectorToPlane(Vector3 inVect, Vector3 normal)
    {
        return Vector3.ProjectOnPlane(inVect, normal);
    }

    protected void UpdateMovement(float moveFloatiness)
    {
        float dot = Vector3.Dot(controller.FinalMove.normalized, moveAmount.normalized);
        if (dot < 0)
            moveAmount = -moveAmount;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMove, ref smoothMove, moveFloatiness);
        controller.FinalMove = moveAmount;
    }

    public void TraceDebug()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
        Debug.DrawRay(position, moveAmount, Color.red);
        Debug.DrawRay(position, targetMove, Color.green);
        Debug.DrawRay(position, controller.FinalMove, Color.blue);
    }
}
