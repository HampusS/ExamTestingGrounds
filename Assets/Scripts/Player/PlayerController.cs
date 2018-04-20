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
    public MoveStates currMoveState { get; private set; }
    public MoveStates prevMoveState { get; private set; }
    public BaseState currentState { get; private set; }
    List<BaseState> states;

    [SerializeField]
    LayerMask wallLayer;
    [SerializeField]
    float health;

    public float moveSpeed = 10;
    public float jumpStrength = 8;
    public float jumpHeight = 300;
    public float turnAroundSpeed = 6;
    public float maxSpeed = 15;

    float rayLengthHorizontal = 0.6f;
    float rayLengthVertical = 1.25f;

    public bool onForwardWall { get; private set; }
    public bool onRightWall { get; private set; }
    public bool onLeftWall { get; private set; }
    public bool onBottom { get; private set; }
    public bool onTop { get; private set; }
    public bool onGravityMultiplier { get; set; }

    public RaycastHit HorizontalHit() { return horizHit; }
    public RaycastHit BottomRayHit() { return bottomHit; }
    public RaycastHit TopRayHit() { return topHit; }
    RaycastHit bottomHit, topHit, horizHit;
    Rigidbody rgdBody;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        rgdBody = GetComponent<Rigidbody>();
        states = new List<BaseState>();
        states.Add(GetComponent<AirState>());
        states.Add(GetComponent<GroundState>());
        states.Add(GetComponent<WallRun>());
        states.Add(GetComponent<WallClimb>());
        states.Add(GetComponent<LedgeGrab>());

        currentState = states[0];
        currMoveState = MoveStates.AIR;
    }

    void Update()
    {
        RayTrace();
        prevMoveState = currMoveState;
        if (currentState.Exit())
        {
            foreach (BaseState state in states)
            {
                if (state.Enter())
                {
                    currMoveState = state.myStateType;
                    currentState = state;
                }
            }
        }
        currentState.Run();
        if (Input.GetKey(KeyCode.LeftControl))
            transform.localScale = new Vector3(1, 0.65f, 1);
        else if (!onTop)
            transform.localScale = new Vector3(1, 1, 1);

        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;

    }

    private void FixedUpdate()
    {
        if (onGravityMultiplier)
        {
            if (rgdBody.velocity.y < 0 || rgdBody.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rgdBody.velocity += Vector3.up * Physics.gravity.y * 2 * Time.deltaTime;
            }
        }
    }

    void RayTrace()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(ray.origin, transform.up * (rayLengthVertical + 0.15f), Color.black);
        Debug.DrawRay(ray.origin, -transform.up * rayLengthVertical, Color.black);

        Debug.DrawRay(ray.origin, -transform.right * rayLengthHorizontal, Color.black);
        Debug.DrawRay(ray.origin, transform.right * rayLengthHorizontal, Color.black);
        Debug.DrawRay(ray.origin, transform.forward * rayLengthHorizontal, Color.black);

        Debug.DrawRay(ray.origin, (-transform.right + transform.forward).normalized * rayLengthHorizontal, Color.black);
        Debug.DrawRay(ray.origin, (transform.right + transform.forward).normalized * rayLengthHorizontal, Color.black);
        Debug.DrawRay(ray.origin, (-transform.right + -transform.forward).normalized * rayLengthHorizontal, Color.black);
        Debug.DrawRay(ray.origin, (transform.right + -transform.forward).normalized * rayLengthHorizontal, Color.black);

        onBottom = Physics.Raycast(ray.origin, -transform.up, out bottomHit, rayLengthVertical);
        onTop = Physics.Raycast(ray.origin, transform.up, out topHit, (rayLengthVertical + 0.15f));
        onLeftWall = Physics.Raycast(ray.origin, -transform.right, out horizHit, rayLengthHorizontal, wallLayer);
        onRightWall = Physics.Raycast(ray.origin, transform.right, out horizHit, rayLengthHorizontal, wallLayer);
        onForwardWall = Physics.Raycast(ray.origin, transform.forward, out horizHit, rayLengthHorizontal, wallLayer);

        if (!onLeftWall && !onRightWall)
        {
            onLeftWall = Physics.Raycast(ray.origin, (-transform.right + transform.forward).normalized, out horizHit, rayLengthHorizontal, wallLayer);
            onRightWall = Physics.Raycast(ray.origin, (transform.right + transform.forward).normalized, out horizHit, rayLengthHorizontal, wallLayer);
            if (!onLeftWall && !onRightWall)
            {
                onLeftWall = Physics.Raycast(ray.origin, (-transform.right + -transform.forward).normalized, out horizHit, rayLengthHorizontal, wallLayer);
                onRightWall = Physics.Raycast(ray.origin, (transform.right + -transform.forward).normalized, out horizHit, rayLengthHorizontal, wallLayer);
            }
        }
    }
    
}
