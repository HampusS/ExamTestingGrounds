using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwarmBase : MonoBehaviour{
    protected SwarmerController controller;
    public SwarmState stateType { get; set; }

    void Start()
    {
        controller = GetComponent<SwarmerController>();
        stateType = SwarmState.ERROR;
    }

    public abstract bool Enter();
    public abstract void Run();
    public abstract bool Exit();
}
