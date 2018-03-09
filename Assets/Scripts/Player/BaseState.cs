using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseState : MonoBehaviour {
    protected Rigidbody rgdBody;
    protected PlayerController controller;
    public MoveStates myStateType { get; set; }

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        rgdBody = GetComponent<Rigidbody>();
        myStateType = MoveStates.ERROR;
    }

    public abstract bool Enter();
    public abstract void Run();
    public abstract bool Exit();
}
