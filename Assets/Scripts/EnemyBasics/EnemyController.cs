using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTasks
{
    ERROR,
    IDLE,
    MOVE,
    ATTACK,
}

public class EnemyController : MonoBehaviour
{
    public GameObject player { get; private set; }
    public Animator anim;

    public float health = 2;
    [SerializeField]
    protected float aggroRange = 180;
    [SerializeField]
    protected float attackRange = 2.5f;

    protected EnemyTasks currentTask;
    protected EnemyBase currentState;
    protected List<EnemyBase> states;

    public Vector3 Destination { get; set; }
    public bool isAlive()
    {
        return health != 0;
    }

    public bool InAttackRange()
    {
        return (Vector3.Distance(transform.position, player.transform.position) < attackRange);
    }

    public bool InAggroRange()
    {
        return (Vector3.Distance(transform.position, player.transform.position) < aggroRange);
    }

    public bool InAggroSight()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, (player.transform.position - transform.position), out hit);
        if (hit.transform != null && hit.transform.tag != null)
            return (hit.transform.tag == "Player");
        return false;
    }

    public void UpdateDestination()
    {
        Destination = player.transform.position;
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        currentTask = EnemyTasks.ERROR;
        states = new List<EnemyBase>();
        states.Add(GetComponent<EnemyIdleState>());
        states.Add(GetComponent<EnemyNavMoveState>());
        states.Add(GetComponent<EnemyAttackState>());
        currentState = states[0];
    }

    void Update()
    {
        StateMachine();
    }

    protected virtual void StateMachine()
    {
        if (currentState.Exit())
        {
            foreach (EnemyBase state in states)
            {
                if (state.Enter())
                {
                    currentTask = state.taskType;
                    currentState = state;
                }
            }
        }
        //Debug.Log(currentState);
        currentState.Run();
        if (GetComponent<Rigidbody>().velocity.magnitude > 0)
            GetComponent<Rigidbody>().AddForce(-GetComponent<Rigidbody>().velocity * 4, ForceMode.Acceleration);

        if (!isAlive())
            KillMe();
    }

    public void Damage(float amount)
    {
        if (health > 0 && health - amount <= 0)
            health = 0;
        else
            health -= amount;
    }

    public void KillMe()
    {
        Destroy(gameObject);
    }
}
