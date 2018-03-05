using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveStates
{
    ERROR,
    AIRBORNE,
    GROUND,
    WALLRUN,
    LEDGEGRAB,
}

public class PlayerController : MonoBehaviour
{
    public MoveStates moveStates { get; set; }
    public BaseState currentState { get; private set; }
    List<BaseState> states;

    Rigidbody rgdBody;
    Vector3 moveAmount;
    Vector3 finalMove;
    Vector3 smoothMove;
    public RaycastHit BottomRayHit() { return bottomHit; }
    RaycastHit bottomHit;

    bool onBottom;

    public float rayLengthHorizontal = 0.6f;
    public float rayLengthVertical = 1.25f;

    void Start()
    {
        //if (Input.GetKeyDown("escape"))
        //    Cursor.lockState = CursorLockMode.None;
        //Cursor.lockState = CursorLockMode.Locked;
        rgdBody = GetComponent<Rigidbody>();
        states = new List<BaseState>();
        states.Add(GetComponent<GroundState>());
        states.Add(GetComponent<WallRun>());
        moveStates = MoveStates.GROUND;
    }

    void Update()
    {
        RayTraceBottom();
        if (currentState == null || currentState.Exit())
        {
            UpdateGroundCheck();
            currentState = null;
            foreach (BaseState state in states)
            {
                if (state.Enter())
                {
                    currentState = state;
                }
            }
        }
        if (currentState != null)
            currentState.Run();

        //Debug.Log(moveStates);

        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(ray.origin, -transform.up * rayLengthVertical, Color.black);
        Debug.DrawRay(ray.origin, -transform.right * rayLengthHorizontal, Color.black);
        Debug.DrawRay(ray.origin, transform.right * rayLengthHorizontal, Color.black);
        Debug.DrawRay(ray.origin, transform.forward * rayLengthHorizontal, Color.black);

        //Debug.Log(transform.TransformDirection(moveAmount) + " " + moveAmount);

    }

    public void UpdateMoveAmount(float speed, float moveFloatiness)
    {
        Vector3 strafe = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, strafe * speed, ref smoothMove, moveFloatiness);
        finalMove = transform.TransformDirection(moveAmount);
    }

    public void UpdateGroundCheck()
    {
        if (moveStates != MoveStates.GROUND && onBottom)
        {
            moveStates = MoveStates.GROUND;
            UpdateMoveAmount(0, 0.05f);
        }
    }

    void RayTraceBottom()
    {
        onBottom = Physics.Raycast(transform.position, -transform.up, out bottomHit, 1.25f);
    }

    private void FixedUpdate()
    {
        if (moveStates != MoveStates.WALLRUN)
            if (rgdBody.velocity.y < 0 || rgdBody.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rgdBody.velocity += Vector3.up * Physics.gravity.y * 3 * Time.deltaTime;
            }

        rgdBody.MovePosition(rgdBody.position + finalMove * Time.fixedDeltaTime);
    }
}
