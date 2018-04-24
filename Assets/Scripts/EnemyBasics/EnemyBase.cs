using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour {
    public EnemyTasks taskType { get; set; }
    protected EnemyController controller;
    protected NavMeshAgent navMesh;

    void Awake()
    {
        controller = GetComponent<EnemyController>();
        navMesh = GetComponent<NavMeshAgent>();
        taskType = EnemyTasks.ERROR;
    }

    public abstract bool Enter();
    public abstract void Run();
    public abstract bool Exit();
}
