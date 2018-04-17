using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour {
    public EnemyTasks taskType { get; set; }
    protected EnemyController controller;

    void Start()
    {
        controller = GetComponent<EnemyController>();
        taskType = EnemyTasks.ERROR;
    }

    public abstract bool Enter();
    public abstract void Run();
    public abstract bool Exit();
}
