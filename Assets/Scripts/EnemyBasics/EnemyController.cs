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
    public GameObject DeathEffect;
    public GameObject DamagedEffect;
    GameObject smoke;
    bool damaged = false;

    public float health = 2;
    float startHealth;
    [SerializeField]
    protected float aggroRange = 180;
    [SerializeField]
    protected float attackRange = 2.5f;

    protected EnemyTasks currentTask;
    protected EnemyBase currentState;
    protected List<EnemyBase> states;

    public PlayerController playerControl { get; set; }

    public Vector3 Destination { get; set; }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerControl = player.GetComponent<PlayerController>();
        startHealth = health;
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
        if (!damaged)
        {
            damaged = true;
            smoke = Instantiate(DamagedEffect, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), transform.rotation);
            smoke.transform.parent = gameObject.transform;
        }
        ParticleSystem.EmissionModule em = smoke.GetComponent<ParticleSystem>().emission;
        Debug.Log(100 * ((startHealth - health) / startHealth) + " " + health);
        em.rateOverTime = 100 * ((startHealth - health) / startHealth);
    }

    public void KillMe()
    {
        GameObject clone = Instantiate(DeathEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
        Destroy(gameObject);
        Destroy(clone, 5);
        Destroy(smoke);
    }

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
}
