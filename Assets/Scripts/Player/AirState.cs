using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : BaseState
{
    [SerializeField]
    float speed = 4;
    [SerializeField]
    float moveFloatiness = .35f;

    // Use this for initialization
    void Start()
    {
        myStateType = MoveStates.AIR;
    }

    public override bool Enter()
    {
        if (!controller.onBottom && controller.moveStates != MoveStates.AIR)
            return true;
        return false;
    }

    public override void Run()
    {

        controller.UpdateMoveAmount(speed, moveFloatiness);
    }

    public override bool Exit()
    {
        return true;
    }
}
