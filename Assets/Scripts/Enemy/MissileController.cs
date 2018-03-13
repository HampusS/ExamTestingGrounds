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
    //public Transform target;
    //public float speed;
    //public float maxSpeed;
    //public float rotateSpeed; 
    Ray rayFwd;
    Ray rayLeft;
    Ray rayRight;
    public float rayAngle;
    private float dist;
    public Transform target;
    public Rigidbody rb { get; set; }
    public Vector3 steering { get; set; }

    Vector3 right;
    Vector3 left;
    public float rayLenght = 10;

    public RocketState NPCState { get; set; }
    public BaseEnemyState currentState { get; private set; }
    List<BaseEnemyState> states;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        states = new List<BaseEnemyState>();
        states.Add(GetComponent<Chase>());
        currentState = states[0];
        NPCState = RocketState.CHASE;
    }
    void Update()
    {
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
        UpdateRays();
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
        right = transform.forward - transform.right * (-rayAngle);
        left = transform.forward - transform.right * (rayAngle);
        rayFwd = new Ray(transform.position, Vector3.forward);
        rayLeft = new Ray(transform.position, left * rayLenght);
        rayRight = new Ray(transform.position, right * rayLenght);
        if(Physics.Raycast(transform.position, Vector3.forward, rayLenght))
        {
            Debug.Log("rayhit");
            
        }

        Debug.DrawRay(rayFwd.origin, transform.forward * rayLenght, Color.blue);
        Debug.DrawRay(rayFwd.origin, right * rayLenght, Color.red);
        Debug.DrawRay(rayFwd.origin, left * rayLenght, Color.green);
    }

    bool Rayhit()
    {
        if (Physics.Raycast(rayFwd, rayLenght))
        {
            Debug.Log("rayhit");
            return true;
        }
        return false;
    }
}
