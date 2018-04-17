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
    float health;
    [SerializeField]
    public float moveSpeed = 10;
    public float jumpStrength = 8;
    public float jumpHeight = 300;
    public float turnAroundSpeed = 6; 

    float rayLengthHorizontal = 0.7f;
    float rayLengthVertical = 1.25f;

    public MoveStates currMoveState { get; private set; }
    public MoveStates prevMoveState { get; private set; }

    public BaseState currentState { get; private set; }
    List<BaseState> states;

    public Vector3 FinalMove { get; set; }
    public Vector3 moveAmount { get; set; }
    public Vector3 targetMove { get; set; }
    public bool onForwardWall { get; private set; }
    public bool onRightWall { get; private set; }
    public bool onLeftWall { get; private set; }
    public bool onBottom { get; private set; }
    public bool onTop { get; private set; }
    public bool onGravityMultiplier { get; set; }

    public float halfHeight { get; set; }
    public float fullHeight { get; set; }

    public RaycastHit HorizontalHit() { return horizHit; }
    public RaycastHit BottomRayHit() { return bottomHit; }
    public RaycastHit TopRayHit() { return topHit; }
    RaycastHit bottomHit, topHit, horizHit;
    Rigidbody rgdBody;

    // Add invincibility frames
    // Add Ammo
    // Add Health

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
    }

    private void FixedUpdate()
    {
        if (onGravityMultiplier)
            if (rgdBody.velocity.y < 0 || rgdBody.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rgdBody.velocity += Vector3.up * Physics.gravity.y * 3 * Time.deltaTime;
            }
        rgdBody.MovePosition(rgdBody.position + FinalMove * Time.fixedDeltaTime);
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
        
        if(!onLeftWall && !onRightWall)
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



    public void JumpAway(Vector3 direction, float height, float perpendicularStrength)
    {
        SetMoveAmount(direction * perpendicularStrength);
        Jump(height);
        //moveAmount = Vector3.Project(-moveAmount, controller.HorizontalHit().normal);
    }

    void SetMoveAmount(Vector3 newVect)
    {
        targetMove = Vector3.zero;
        moveAmount = newVect;
        FinalMove = newVect;
    }

    void Jump(float magnitude)
    {
        rgdBody.velocity = Vector3.zero;
        rgdBody.AddForce(transform.up * magnitude);
    }

}
