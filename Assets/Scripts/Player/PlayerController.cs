using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveStates
{
    ERROR,
    AIR,
    GROUND,
    WALLRUN,
    LEDGEGRAB,
}

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    LayerMask wallLayer;
    public MoveStates moveStates { get; set; }
    public BaseState currentState { get; private set; }
    List<BaseState> states;

    public RaycastHit BottomRayHit() { return bottomHit; }
    public RaycastHit HorizontalHit() { return horizHit; }
    public Vector3 finalMove { get; set; }
    public bool onBottom { get; set; }
    public bool onLeftWall { get; set; }
    public bool onRightWall { get; set; }
    public bool onForwardWall { get; set; }

    RaycastHit bottomHit, horizHit;
    Vector3 moveAmount;
    Vector3 smoothMove;
    Rigidbody rgdBody;


    public float rayLengthHorizontal = 0.6f;
    public float rayLengthVertical = 1.25f;

    void Start()
    {
        //if (Input.GetKeyDown("escape"))
        //    Cursor.lockState = CursorLockMode.None;
        //Cursor.lockState = CursorLockMode.Locked;
        rgdBody = GetComponent<Rigidbody>();
        states = new List<BaseState>();
        //states.Add(GetComponent<AirState>());
        states.Add(GetComponent<GroundState>());
        states.Add(GetComponent<WallRun>());
        moveStates = MoveStates.GROUND;
    }

    void Update()
    {
        RayTrace();

        if (currentState == null || currentState.Exit())
        {
            BumpGround();
            currentState = null;
            foreach (BaseState state in states)
            {
                if (state.Enter())
                {
                    moveStates = state.myStateType;
                    currentState = state;
                }
            }
        }
        if (currentState != null)
            currentState.Run();

        DrawRayTraces();
    }

    public void UpdateMoveAmount(float speed, float moveFloatiness)
    {
        Vector3 strafe = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, strafe * speed, ref smoothMove, moveFloatiness);
        finalMove = transform.TransformDirection(moveAmount);
    }

    public void SetMoveAmount(Vector3 newVect)
    {
        moveAmount = newVect;
        finalMove = newVect;
    }

    public void BumpGround()
    {
        if (moveStates != MoveStates.GROUND && onBottom)
        {
            moveStates = MoveStates.GROUND;
            UpdateMoveAmount(0, 0.05f);
        }
    }

    void RayTrace()
    {
        onBottom = Physics.Raycast(transform.position, -transform.up, out bottomHit, 1.25f);
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
    
    private void FixedUpdate()
    {
        if (rgdBody.velocity.y < 0 || rgdBody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rgdBody.velocity += Vector3.up * Physics.gravity.y * 3 * Time.deltaTime;
        }

        rgdBody.MovePosition(rgdBody.position + finalMove * Time.fixedDeltaTime);
    }
}
