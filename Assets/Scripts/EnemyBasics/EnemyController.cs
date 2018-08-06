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
    public bool damaged { get; set; }

    public Rigidbody rgdBody { get; set; }
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
    [SerializeField]
    GameObject Loot;
    [SerializeField]
    int money;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerControl = player.GetComponent<PlayerController>();
        rgdBody = GetComponent<Rigidbody>();
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
        if (rgdBody.velocity.magnitude > 0)
            rgdBody.AddForce(-rgdBody.velocity * 4, ForceMode.Acceleration);
        if (!isAlive())
            KillMe();
    }

    public void Damage(float amount)
    {
        // PLAY HIT SOUND (& HIT ANIMATION?)
        if (health > 0 && health - amount <= 0)
        {
            health = 0;
        }
        else
            health -= amount;
        if (!damaged)
        {
            damaged = true;
        }
    }

    public void KnockBack(Vector3 source)
    {
        Vector3 vect = transform.position - source;
        vect.Normalize();
        vect += Vector3.up;
        vect *= 10;
        rgdBody.AddForce(vect, ForceMode.Impulse);
    }

    public void KillMe()
    {
        GameObject deathParticles = Instantiate(DeathEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
        //playerControl.Currency += (int)Random.Range(money * 0.85f, money * 1.15f);
        if (Loot != null)
        {
            float rand = Random.Range(0f, 1f);

            if (rand <= 0.25f)
                Instantiate(Loot, transform.position, transform.rotation);
        }
        Destroy(gameObject);
        Destroy(deathParticles, 5);
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
