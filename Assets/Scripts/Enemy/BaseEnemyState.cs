using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseEnemyState : MonoBehaviour {
    protected MissileController controller;
    public RocketState myStateType { get; set; }

    private void Awake()
    {
        controller = GetComponent<MissileController>();
        myStateType = RocketState.ERROR;
    }

    public abstract bool Enter();
    public abstract void Run();
    public abstract bool Exit();
}
