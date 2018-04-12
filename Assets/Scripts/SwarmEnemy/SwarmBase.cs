using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwarmBase : MonoBehaviour{
    public SwarmState stateType { get; set; }
    protected SwarmerController controller;

    void Start()
    {
        controller = GetComponent<SwarmerController>();
        stateType = SwarmState.ERROR;
    }

    public abstract bool Enter();
    public abstract void Run();
    public abstract bool Exit();
}
