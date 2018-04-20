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
    public float jumpHeight = 30;
    public float turnAroundSpeed = 6;
    public float maxSpeed = 15;

    float rayLengthHorizontal = 0.6f;
    float rayLengthVertical = 1.1f;

    public bool onForwardWall { get; private set; }
    public bool onRightWall { get; private set; }
    public bool onLeftWall { get; private set; }
    public bool onBottom { get; private set; }

    public bool onGravityMultiplier { get; set; }

    public RaycastHit HorizontalHit() { return horizHit; }
    public RaycastHit BottomRayHit() { return bottomHit; }
    RaycastHit bottomHit, horizHit;
    Rigidbody rgdBody;


    bool crouched = false;


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
        Debug.Log(currMoveState);
        currentState.Run();
        Crouching();


        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;
    }

    private void FixedUpdate()
    {
        if (onGravityMultiplier)
        {
            if (rgdBody.velocity.y < 0 || rgdBody.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rgdBody.velocity += Vector3.up * Physics.gravity.y * 3 * Time.deltaTime;
            }
        }
    }

    void Crouching()
    {
        if (!crouched && Input.GetKey(KeyCode.LeftControl))
        {
            CapsuleCollider capsule = GetComponent<CapsuleCollider>();
            capsule.height = 1;
            crouched = true;
        }
        else
        {
            if (crouched)
            {
                Debug.DrawRay(transform.position, transform.up * (rayLengthVertical + 0.5f), Color.black);
                bool onTop = Physics.Raycast(transform.position, transform.up, (rayLengthVertical + 0.5f));
                if (!onTop && !Input.GetKey(KeyCode.LeftControl))
                {
                    CapsuleCollider capsule = GetComponent<CapsuleCollider>();
                    capsule.height = 2;
                    crouched = false;
                }
            }
        }
    }

    void RayTrace()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        Debug.DrawRay(ray.origin, -transform.up * rayLengthVertical, Color.black);
        onBottom = Physics.Raycast(ray.origin, -transform.up, out bottomHit, rayLengthVertical);
        
        Debug.DrawRay(ray.origin, -transform.right * rayLengthHorizontal, Color.black);
        onLeftWall = Physics.Raycast(ray.origin, -transform.right, out horizHit, rayLengthHorizontal, wallLayer);

        Debug.DrawRay(ray.origin, transform.right * rayLengthHorizontal, Color.black);
        onRightWall = Physics.Raycast(ray.origin, transform.right, out horizHit, rayLengthHorizontal, wallLayer);

        Debug.DrawRay(ray.origin, transform.forward * rayLengthHorizontal, Color.black);
        onForwardWall = Physics.Raycast(ray.origin, transform.forward, out horizHit, rayLengthHorizontal, wallLayer);

        Debug.DrawRay(ray.origin, (-transform.right + transform.forward).normalized * rayLengthHorizontal, Color.black);
        Debug.DrawRay(ray.origin, (transform.right + transform.forward).normalized * rayLengthHorizontal, Color.black);
        Debug.DrawRay(ray.origin, (-transform.right + -transform.forward).normalized * rayLengthHorizontal, Color.black);
        Debug.DrawRay(ray.origin, (transform.right + -transform.forward).normalized * rayLengthHorizontal, Color.black);


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
