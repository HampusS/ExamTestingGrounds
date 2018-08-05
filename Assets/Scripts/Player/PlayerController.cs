using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    float rayLengthHorizontal = 1;
    float rayLengthVertical = 1.1f;

    public bool onForwardWall { get; private set; }
    public bool onBottom { get; private set; }
    public bool onRightWall { get; set; }
    public bool onLeftWall { get; set; }

    public bool MultiplyGravity { get; set; }
    public bool ForceGravity { get; set; }
    public bool LockMovement { get; set; }
    float invulnerableTimer = 0;
    float invulnerableLimit = 1.5f;

    public bool CanJump { get; set; }
    
    float hpupTimer = 1.5f, hpupTime;
    public bool DisplayHpUp { get; set; }
    public Transform shopPivot;

    public RaycastHit HorizontalHit()
    {
        if (onRightWall)
            return rightHit;
        else if (onLeftWall)
            return leftHit;
        else if (onForwardWall)
            return forwardHit;

        return new RaycastHit();
    }
    public RaycastHit BottomRayHit() { return bottomHit; }
    public RaycastHit bottomHit, forwardHit, rightHit, leftHit;
    Rigidbody rgdBody;
    public CapsuleCollider capsule { get; set; }


    public bool Crouch { get; set; }
    public float crouchSpeed = 8;

    float lerpHp = 0;
    float Health = 100;
    float maxHp;
    public bool isInvulnerable { get; set; }

    public int Currency { get; set; }

    // Animations
    public bool HideWeapon { get; set; }
    public bool isRunning { get; set; }


    public bool isAlive()
    {
        return Health != 0;
    }

    public static PlayerController Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        capsule = GetComponent<CapsuleCollider>();
        //flicker = GameObject.Find("Canvas").GetComponent<FlickerHealth>();
        rgdBody = GetComponent<Rigidbody>();
        states = new List<BaseState>();
        states.Add(GetComponent<AirState>());
        states.Add(GetComponent<GroundState>());
        states.Add(GetComponent<WallRun>());
        states.Add(GetComponent<WallClimb>());
        states.Add(GetComponent<LedgeGrab>());

        currentState = states[0];
        currMoveState = MoveStates.AIR;
        LockMovement = false;

        maxHp = Health;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            isInvulnerable = true;
        RayTrace();
        Crouching();
        UpdateHudHp();
        Invulnerable();
        PingPongHpUp();
        PlayerDeath();

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

        if (CanJump && Input.GetButtonDown("Jump"))
        {
            rgdBody.velocity = new Vector3(rgdBody.velocity.x, 0, rgdBody.velocity.z);
            rgdBody.AddForce(transform.up * (jumpHeight * 0.925f), ForceMode.VelocityChange);
            CanJump = false;
            AudioM.Instance.Stop("step");
        }

        if (isRunning && onBottom)
        {
            Sound s = AudioM.Instance.FindSound("step");
            if (!s.source.isPlaying)
            {
                s.source.pitch = Random.Range(2f, 2.5f);
                s.source.volume = Random.Range(0.1f, 0.3f);
                s.source.Play();
            }
        }
        else if(isRunning && currentState.myStateType == MoveStates.WALLRUN)
        {
            Sound s = AudioM.Instance.FindSound("step");
            if (!s.source.isPlaying)
            {
                s.source.pitch = Random.Range(3f, 3.5f);
                s.source.volume = Random.Range(0.1f, 0.3f);
                s.source.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
            GameObject.Find("ScenesManager").GetComponent<SceneHandler>().LoadHub();
    }

    private void FixedUpdate()
    {
        if (MultiplyGravity)
        {
            if (rgdBody.velocity.y < 0 || rgdBody.velocity.y > 0 && !Input.GetButton("Jump") || ForceGravity && rgdBody.velocity.y > 0)
            {
                rgdBody.velocity += Vector3.up * Physics.gravity.y * 3 * Time.deltaTime;
            }
        }
    }

    void PlayerDeath()
    {
        if (!isAlive())
        {
            ShadeController.Instance.TriggerHub();
            AddHealth(100);
        }
    }

    public void Invulnerable()
    {
        if (isInvulnerable)
        {
            invulnerableTimer += Time.deltaTime;
            CanvasManager.Instance.flickerHP.FlickerHp();
            if (invulnerableTimer > invulnerableLimit)
            {
                isInvulnerable = false;
                invulnerableTimer = 0;
                CanvasManager.Instance.flickerHP.ResetColor();
            }
        }
    }

    public void CancelInvulnerability()
    {
        invulnerableTimer = invulnerableLimit;
    }

    private void UpdateHudHp()
    {
        if (lerpHp != Health)
        {
            lerpHp = Mathf.Lerp(lerpHp, Health, Time.deltaTime * 5);
            if (Mathf.RoundToInt(lerpHp) == Health)
                lerpHp = Health;
            CanvasManager.Instance.hpText.text = ((int)lerpHp).ToString();
        }
    }

    public void AddHealth(float amount)
    {
        if (Health + amount < maxHp)
        {
            Health += amount;
            CanvasManager.Instance.hpUpText.text = "+" + amount.ToString();
        }
        else if(Health == maxHp)
        {
            CanvasManager.Instance.hpUpText.text = "FULL";
        }
        else
        {
            amount = maxHp - Health;
            CanvasManager.Instance.hpUpText.text = "+" + amount.ToString();
            Health = maxHp;
        }
        DisplayHpUp = true;
    }

    public void DamagePlayer(float amount)
    {
        GetComponentInChildren<CamStates>().onShake = true;
        isInvulnerable = true;
        if (Health > 0 && Health - amount < 0)
            Health = 0;
        else
            Health -= amount;
    }

    public void KnockBack(Vector3 direction, float amount)
    {
        rgdBody.AddForce(transform.up * (jumpHeight * 0.9f), ForceMode.VelocityChange);
        rgdBody.velocity = direction * amount;
    }

    void Crouching()
    {
        bool input = Input.GetKey(KeyCode.LeftControl);
        if (input)
        {
            capsule.height = 1;
            MultiplyGravity = true;
            GetComponentInChildren<CameraControls>().CrouchCam();
        }
        else if (Crouch)
        {
            //Up
            //Debug.DrawRay(transform.position, transform.up * (rayLengthVertical + 0.5f), Color.red);
            bool onTop = Physics.Raycast(transform.position, transform.up, (rayLengthVertical + 0.5f));
            if (!onTop)
            {
                //Debug.DrawRay(transform.position + (transform.forward * 0.5f), transform.up * (rayLengthVertical + 0.5f), Color.red);
                onTop = Physics.Raycast(transform.position + (transform.forward * 0.5f), transform.up, (rayLengthVertical + 0.5f));
                if (!onTop)
                {
                    //Debug.DrawRay(transform.position + (-transform.forward * 0.5f), transform.up * (rayLengthVertical + 0.5f), Color.red);
                    onTop = Physics.Raycast(transform.position + (-transform.forward * 0.5f), transform.up, (rayLengthVertical + 0.5f));
                    if (!onTop)
                    {
                        //Debug.DrawRay(transform.position + (transform.right * 0.5f), transform.up * (rayLengthVertical + 0.5f), Color.red);
                        onTop = Physics.Raycast(transform.position + (transform.right * 0.5f), transform.up, (rayLengthVertical + 0.5f));
                        if (!onTop)
                        {
                            //Debug.DrawRay(transform.position + (-transform.right * 0.5f), transform.up * (rayLengthVertical + 0.5f), Color.red);
                            onTop = Physics.Raycast(transform.position + (-transform.right * 0.5f), transform.up, (rayLengthVertical + 0.5f));
                        }
                    }
                }
            }
            if (!onTop)
            {
                capsule.height = 2;
                MultiplyGravity = false;
                GetComponentInChildren<CameraControls>().StandCam();
            }
            else
                input = true;
        }
        Crouch = input;
    }

    void RayTrace()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        //Down
        //Debug.DrawRay(ray.origin, -transform.up * rayLengthVertical, Color.red);
        onBottom = Physics.Raycast(ray.origin, -transform.up, out bottomHit, rayLengthVertical);

        //Left
        //Debug.DrawRay(ray.origin, -transform.right * rayLengthHorizontal, Color.red);
        onLeftWall = Physics.Raycast(ray.origin, -transform.right, out leftHit, rayLengthHorizontal, wallLayer);

        //Right
        //Debug.DrawRay(ray.origin, transform.right * rayLengthHorizontal, Color.red);
        onRightWall = Physics.Raycast(ray.origin, transform.right, out rightHit, rayLengthHorizontal, wallLayer);

        //Forward
        //Debug.DrawRay(ray.origin, transform.forward * rayLengthHorizontal, Color.red);
        onForwardWall = Physics.Raycast(ray.origin, transform.forward, out forwardHit, rayLengthHorizontal, wallLayer);
    }
    
    public void PingPongHpUp()
    {
        if (DisplayHpUp)
        {
            hpupTime += Time.deltaTime;
            CanvasManager.Instance.hpUpText.color = new Color(CanvasManager.Instance.hpUpText.color.r, CanvasManager.Instance.hpUpText.color.g, CanvasManager.Instance.hpUpText.color.b, Mathf.PingPong(Time.time * 2, 1));
            if(hpupTime > hpupTimer)
            {
                DisplayHpUp = false;
                hpupTime = 0;
                CanvasManager.Instance.hpUpText.color = new Color(CanvasManager.Instance.hpUpText.color.r, CanvasManager.Instance.hpUpText.color.g, CanvasManager.Instance.hpUpText.color.b, 0);
            }
        }
    }
}
