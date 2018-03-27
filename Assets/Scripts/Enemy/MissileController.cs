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
    public bool downHitBool { get; set; }
    public bool rightHitBool { get; set; }
    public bool leftHitBool { get; set; }
    public bool upHitBool { get; set; }

    //public RaycastHit RightRayHit() { return hitRight; }
    //public RaycastHit LeftRayHit() { return hitLeft; }
    public RaycastHit Hit() { return hit; }
    public RaycastHit DownUpHit() { return upDownHit; }
    public Vector3 rightRayVector { get; private set; }
    public Vector3 leftRayVector { get; private set; }
    public Vector3 downRayVector { get; private set; }
    public Vector3 upRayVector { get; private set; }
    public RaycastHit hit;
    //public RaycastHit upDownHit;
    public RaycastHit upDownHit;
    //public RaycastHit hitFwd;
    //public RaycastHit hitRight;
    //public RaycastHit hitLeft;
    public float rayLenght = 10;

    public RocketState NPCState { get; set; }
    public BaseEnemyState currentState { get; private set; }
    List<BaseEnemyState> states;


    public Vector3 targetVect { get; set; }
    [SerializeField]
    float rotateSpeed = 1;
    [SerializeField]
    float speed = 20;

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
                }
            }
        }
        currentState.Run();
    }

    void FixedUpdate()
    {
        rb.velocity = transform.forward * speed;
        Quaternion targetRot = Quaternion.LookRotation(targetVect);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed));
        Debug.DrawRay(transform.position, targetVect, Color.cyan, 6);
    }
    void UpdateRays()
    {
        rightRayVector = transform.forward - transform.right * (-rayAngle);
        leftRayVector = transform.forward - transform.right * (rayAngle);
        downRayVector = (transform.forward + (-transform.up)) * (rayAngle);
        upRayVector = (transform.forward + (transform.up)) * (rayAngle);

        upHitBool = Physics.Raycast(transform.position, downRayVector, out upDownHit, rayLenght);
        downHitBool = Physics.Raycast(transform.position, downRayVector, out upDownHit, 1);

        rightHitBool = Physics.Raycast(transform.position, rightRayVector, out hit, rayLenght);
        leftHitBool = Physics.Raycast(transform.position, leftRayVector, out hit, rayLenght);

        Debug.DrawRay(transform.position, downRayVector * rayLenght, Color.blue);
        Debug.DrawRay(transform.position, upRayVector * rayLenght, Color.blue);
        Debug.DrawRay(transform.position, rightRayVector * rayLenght, Color.red);
        Debug.DrawRay(transform.position, leftRayVector * rayLenght, Color.green);
    }

    public bool Rayhit()
    {
        if (downHitBool || rightHitBool || leftHitBool)
        {
            return true;
        }
        else
            return false;
    }
}
