using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public float moveSpeed = 10;
    public float jumpStrength = 8;
    public float jumpHeight = 6;
    public float turnAroundSpeed = 6;

    float rayLengthHorizontal = 0.7f;
    float rayLengthVertical = 1.1f;

    public bool onForwardWall { get; private set; }
    public bool onBottom { get; private set; }
    public bool onRightWall { get; set; }
    public bool onLeftWall { get; set; }

    public bool onGravityMultiplier { get; set; }
    public bool onForceLockMovement { get; set; }
    float invulnerableTimer = 0;
    float invulnerableLimit = 1.75f;

    public RaycastHit HorizontalHit() { return horizHit; }
    public RaycastHit BottomRayHit() { return bottomHit; }
    RaycastHit bottomHit, horizHit;
    Rigidbody rgdBody;


    public bool Crouch { get; set; }
    public float crouchSpeed = 8;

    float lerpHp = 100;
    float trueHp = 100;
    public bool isDamaged { get; set; }
    public Text hpText;
    FlickerHealth flicker;

    public bool isAlive()
    {
        return trueHp != 0;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        flicker = GameObject.Find("Canvas").GetComponent<FlickerHealth>();
        rgdBody = GetComponent<Rigidbody>();
        states = new List<BaseState>();
        states.Add(GetComponent<AirState>());
        states.Add(GetComponent<GroundState>());
        states.Add(GetComponent<WallRun>());
        states.Add(GetComponent<WallClimb>());
        states.Add(GetComponent<LedgeGrab>());

        currentState = states[0];
        currMoveState = MoveStates.AIR;
        onForceLockMovement = false;
        hpText.text = lerpHp.ToString();       
    }

    void Update()
    {
        RayTrace();
        Crouching();
        Invulnerable();
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
        //Debug.Log(currMoveState);
        currentState.Run();

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

    void Invulnerable()
    {
        if (isDamaged)
        {
            invulnerableTimer += Time.deltaTime;
            flicker.FlickerHp();
            if (invulnerableTimer > invulnerableLimit)
            {
                isDamaged = false;
                invulnerableTimer = 0;
                flicker.ResetColor();
            }
        }

        if (lerpHp != trueHp)
        {
            lerpHp = Mathf.Lerp(lerpHp, trueHp, Time.deltaTime * 5);
            hpText.text = ((int)lerpHp).ToString();
            if ((int)lerpHp == trueHp)
                lerpHp = trueHp;
        }
    }



    public void DamagePlayer(float amount)
    {
        isDamaged = true;
        if (trueHp > 0 && trueHp - amount < 0)
            trueHp = 0;
        else
            trueHp -= amount;
    }

    public void KnockBack(Vector3 direction, float amount)
    {
        rgdBody.AddForce(transform.up * (jumpHeight * 0.9f), ForceMode.VelocityChange);
        rgdBody.velocity = direction * amount;
    }

    void Crouching()
    {
        Crouch = Input.GetKey(KeyCode.LeftControl);
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        //Up
        Debug.DrawRay(transform.position, transform.up * (rayLengthVertical + 0.5f), Color.black);
        bool onTop = Physics.Raycast(transform.position, transform.up, (rayLengthVertical + 0.5f));

        if (Crouch)
        {
            capsule.height = 1;
            onGravityMultiplier = true;
        }
        else if (!onTop)
        {
            capsule.height = 2;
            onGravityMultiplier = false;
        }
        else
            Crouch = true;
    }

    void RayTrace()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        //Down
        Debug.DrawRay(ray.origin, -transform.up * rayLengthVertical, Color.black);
        onBottom = Physics.Raycast(ray.origin, -transform.up, out bottomHit, rayLengthVertical);

        //Left
        Debug.DrawRay(ray.origin, -transform.right * rayLengthHorizontal, Color.black);
        onLeftWall = Physics.Raycast(ray.origin, -transform.right, out horizHit, rayLengthHorizontal, wallLayer);

        //Right
        Debug.DrawRay(ray.origin, transform.right * rayLengthHorizontal, Color.black);
        onRightWall = Physics.Raycast(ray.origin, transform.right, out horizHit, rayLengthHorizontal, wallLayer);

        //Forward
        Debug.DrawRay(ray.origin, transform.forward * rayLengthHorizontal, Color.black);
        onForwardWall = Physics.Raycast(ray.origin, transform.forward, out horizHit, rayLengthHorizontal, wallLayer);

        if (!onLeftWall && !onRightWall)
        {
            Debug.DrawRay(ray.origin, (-transform.right + transform.forward).normalized * rayLengthHorizontal, Color.black);
            Debug.DrawRay(ray.origin, (transform.right + transform.forward).normalized * rayLengthHorizontal, Color.black);
            onLeftWall = Physics.Raycast(ray.origin, (-transform.right + transform.forward).normalized, out horizHit, rayLengthHorizontal, wallLayer);
            onRightWall = Physics.Raycast(ray.origin, (transform.right + transform.forward).normalized, out horizHit, rayLengthHorizontal, wallLayer);
        }
    }

    public void UpdateRays()
    {
        if (!onLeftWall && !onRightWall)
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            Debug.DrawRay(ray.origin, (-transform.right + -transform.forward).normalized * rayLengthHorizontal, Color.black);
            Debug.DrawRay(ray.origin, (transform.right + -transform.forward).normalized * rayLengthHorizontal, Color.black);
            onLeftWall = Physics.Raycast(ray.origin, (-transform.right + -transform.forward).normalized, out horizHit, rayLengthHorizontal, wallLayer);
            onRightWall = Physics.Raycast(ray.origin, (transform.right + -transform.forward).normalized, out horizHit, rayLengthHorizontal, wallLayer);
        }
    }
}
