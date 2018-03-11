using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveStates
{
    ERROR,
    AIR,
    GROUND,
    WALLRUN,
    WALLCLIMB,
    LEDGEGRAB,
}

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    LayerMask wallLayer;
    [SerializeField]
    float rayLengthHorizontal = 0.6f;
    [SerializeField]
    float rayLengthVertical = 1.25f;

    public MoveStates moveStates { get; set; }
    public BaseState currentState { get; private set; }
    List<BaseState> states;

    public RaycastHit BottomRayHit() { return bottomHit; }
    public RaycastHit HorizontalHit() { return horizHit; }
    public Vector3 FinalMove { get; set; }
    public bool onForwardWall { get; set; }
    public bool onRightWall { get; set; }
    public bool onLeftWall { get; set; }
    public bool onBottom { get; set; }

    public float halfHeight { get; set; }
    public float fullHeight { get; set; }

    RaycastHit bottomHit, horizHit;
    Vector3 moveAmount;
    Vector3 smoothMove;
    Rigidbody rgdBody;


    void Start()
    {
        //if (Input.GetKeyDown("escape"))
        //    Cursor.lockState = CursorLockMode.None;
        //Cursor.lockState = CursorLockMode.Locked;
        fullHeight = GetComponent<CapsuleCollider>().height;
        halfHeight = fullHeight * 0.5f;
        rgdBody = GetComponent<Rigidbody>();
        states = new List<BaseState>();
        states.Add(GetComponent<AirState>());
        states.Add(GetComponent<GroundState>());
        states.Add(GetComponent<WallRun>());
        states.Add(GetComponent<WallClimb>());
        currentState = states[0];
        moveStates = MoveStates.AIR;
    }

    void Update()
    {
        RayTrace();

        if (currentState.Exit())
        {
            BumpGround();
            foreach (BaseState state in states)
            {
                if (state.Enter())
                {
                    moveStates = state.myStateType;
                    currentState = state;
                }
            }
        }

        currentState.Run();

        DrawRayTraces();
    }

    private void FixedUpdate()
    {
        if (moveStates != MoveStates.WALLRUN && moveStates != MoveStates.WALLCLIMB)
            if (rgdBody.velocity.y < 0 || rgdBody.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rgdBody.velocity += Vector3.up * Physics.gravity.y * 3 * Time.deltaTime;
            }

        rgdBody.MovePosition(rgdBody.position + FinalMove * Time.fixedDeltaTime);
    }

    public void UpdateMoveAmount(float speed, float moveFloatiness)
    {
        Vector3 strafe = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, strafe * speed, ref smoothMove, moveFloatiness);
        FinalMove = transform.TransformDirection(moveAmount);
    }

    public void UpdateMoveAmount(Vector3 dir, float magnitude)
    {
        FinalMove += dir * magnitude;
    }

    public void SetMoveAmount(Vector3 newVect)
    {
        moveAmount = newVect;
        FinalMove = newVect;
    }

    //public void MoveInAir(float speed, float moveFloatiness)
    //{
    //    Vector3 strafe = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    //    if(strafe != Vector3.zero)
    //    {
    //        moveAmount = Vector3.SmoothDamp(moveAmount, strafe * speed, ref smoothMove, moveFloatiness);
    //        FinalMove = transform.TransformDirection(moveAmount);
    //    }
    //}

    public void EnableGravity(bool enable)
    {
        rgdBody.useGravity = enable;
    }

    public void BumpGround()
    {
        if (moveStates != MoveStates.GROUND && onBottom)
        {
            UpdateMoveAmount(0, 0.05f);
        }
    }

    public void Jump(float magnitude)
    {
        rgdBody.velocity = new Vector3(rgdBody.velocity.x, 0, rgdBody.velocity.z);
        rgdBody.AddForce(transform.up * magnitude);
    }

    void RayTrace()
    {
        onBottom = Physics.Raycast(transform.position, -transform.up, out bottomHit, rayLengthVertical);
        onLeftWall = Physics.Raycast(transform.position, -transform.right, out horizHit, rayLengthHorizontal, wallLayer);
        onRightWall = Physics.Raycast(transform.position, transform.right, out horizHit, rayLengthHorizontal, wallLayer);
        onForwardWall = Physics.Raycast(transform.position, transform.forward, out horizHit, rayLengthHorizontal, wallLayer);
    }

    void DrawRayTraces()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(ray.origin, -transform.up * rayLengthVertical, Color.black);
        Debug.DrawRay(ray.origin, -transform.right * rayLengthHorizontal, Color.black);
        Debug.DrawRay(ray.origin, transform.right * rayLengthHorizontal, Color.black);
        Debug.DrawRay(ray.origin, transform.forward * rayLengthHorizontal, Color.black);
    }

    public bool TurnAroundForJump(float height, float perpendicularStrength, float rotateSpeed)
    {
        float dot = Vector3.Dot(transform.forward, HorizontalHit().normal);
        transform.Rotate(Vector3.up, rotateSpeed);

        if (dot > 0.98f)
        {
            UpdateMoveAmount(HorizontalHit().normal, perpendicularStrength);
            Jump(height);
            return true;
        }
        return false;
    }
}
