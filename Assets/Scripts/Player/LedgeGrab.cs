using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrab : BaseState {


    bool onLedge;
    bool onLedgeTraverse;

	// Use this for initialization
    void Start () {
        Initialize();
    }

    void Initialize()
    {
        onLedge = false;
    }

    public override bool Enter()
    {
        return false;
    }

    public override void Run()
    {
        
    }

    public override bool Exit()
    {
        if (onLedge)
            return false;
        return true;
    }

    public void CheckInput()
    {
        if (Input.GetKeyDown("Vertical"))
        {

        }
    }

    public void RayTrace()
    {

    }

    public void ClimbUp()
    {
        if (onLedgeTraverse)
        {
            // Move player to position on top of ledge
        }
    }
}
