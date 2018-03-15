using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RocketState
{
    ERROR,
    IDEL,
    ENDPHASE,
    AVIOD,
    CHASE,
}

[RequireComponent(typeof(Rigidbody))]
public class MissileController : MonoBehaviour
{
    public float rayAngle;
    private float dist;
    public Transform target;
    public Rigidbody rb { get; set; }
    public Vector3 steering { get; set; }
    public bool fwdHitBool { get; set; }
    public bool rightHitBool { get; set; }
    public bool leftHitBool { get; set; }

    public RaycastHit RightRayHit() { return hitRight; }
    public RaycastHit LeftRayHit() { return hitLeft; }
    public RaycastHit FwdRayHit() { return hitFwd; }
    public Vector3 rightRayVector { get; private set; }
    public Vector3 leftRayVector { get; private set; }
    public RaycastHit hitFwd;
    public RaycastHit hitRight;
    public RaycastHit hitLeft;
    public float rayLenght = 10;

    public RocketState NPCState { get; set; }
    public BaseEnemyState currentState { get; private set; }
    List<BaseEnemyState> states;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        states = new List<BaseEnemyState>();
        states.Add(GetComponent<Chase>());
        states.Add(GetComponent<Avoid>());
        currentState = states[0];
        NPCState = RocketState.CHASE;
    }
    void Update()
    {
        UpdateRays();

        if (currentState.Exit())
        {
            foreach (BaseEnemyState state in states)
            {
                if (state.Enter())
                {
                    NPCState = state.myStateType;
                    currentState = state;
                    Debug.Log(NPCState);
                }
            }
        }
        currentState.Run();
    }

    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        rb.AddForce(-steering);
    }
    void UpdateRays()
    {
        rightRayVector = transform.forward - transform.right * (-rayAngle);
        leftRayVector = transform.forward - transform.right * (rayAngle);

        fwdHitBool = Physics.Raycast(transform.position, Vector3.forward, out hitFwd, rayLenght);
        rightHitBool = Physics.Raycast(transform.position, rightRayVector, out hitRight, rayLenght);
        leftHitBool = Physics.Raycast(transform.position, leftRayVector, out hitLeft, rayLenght);
        //Debug.Log("fwd" + fwdHitBool);
        //Debug.Log("right" + rightHitBool);
        //Debug.Log("left" + leftHitBool);
        Debug.DrawRay(transform.position, transform.forward * rayLenght, Color.blue);
        Debug.DrawRay(transform.position, rightRayVector * rayLenght, Color.red);
        Debug.DrawRay(transform.position, leftRayVector * rayLenght, Color.green);
    }

    public bool Rayhit()
    {
        if (fwdHitBool || rightHitBool || leftHitBool)
        {
            return true;
        }
        else
            return false;
    }
}
