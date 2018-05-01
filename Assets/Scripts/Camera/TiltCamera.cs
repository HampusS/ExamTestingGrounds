using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class State
{
    public delegate void Tilt();
    public Tilt run;

    public delegate bool Entry();
    public Entry enter;

    public delegate bool Exit();
    public Exit exit;

    public State(Tilt run, Entry enter, Exit exit)
    {
        this.enter = enter;
        this.exit = exit;
        this.run = run;
    }
}

public class TiltCamera : MonoBehaviour
{
    [SerializeField]
    float lerpSpeed = 2;
    [SerializeField]
    float degrees = 20;

    public bool onLeft { get; set; }
    public bool onRight { get; set; }
    public bool onAlign { get; set; }

    List<State> states = new List<State>();

    void Start()
    {
        states.Add(new State(AlignCamera, Align, isAligned));
        states.Add(new State(TiltCameraLeft, Left, isLeft));
        states.Add(new State(TiltCameraRight, Right, isRight));
    }

    private void Update()
    {
        foreach (State state in states)
        {
            if (state.enter() /*&& !state.exit()*/)
                state.run();
        }
    }

    void TiltCameraLeft()
    {
        Debug.Log("Left");

        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.AngleAxis(degrees, Vector3.forward), Time.deltaTime * lerpSpeed);
    }

    void TiltCameraRight()
    {
        Debug.Log("Right");

        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.AngleAxis(-degrees, Vector3.forward), Time.deltaTime * lerpSpeed);
    }

    void AlignCamera()
    {
        Debug.Log("Align");
        if (transform.localRotation != Quaternion.AngleAxis(0, Vector3.forward))
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.AngleAxis(0, Vector3.forward), Time.deltaTime * lerpSpeed);
    }

    public void ResetCamera()
    {
        onLeft = false;
        onRight = false;
        onAlign = false;
    }

    bool Align()
    {
        return onAlign;
    }

    bool Right()
    {
        return onRight;
    }

    bool Left()
    {
        return onLeft;
    }

    bool isAligned()
    {
        if (transform.localRotation != Quaternion.AngleAxis(0, Vector3.forward))
        {
            onAlign = false;
            return true;
        }
        return false;
    }

    bool isLeft()
    {
        if (transform.localRotation != Quaternion.AngleAxis(degrees, Vector3.forward))
        {
            onLeft = false;
            return true;
        }
        return false;
    }

    bool isRight()
    {
        if (transform.localRotation != Quaternion.AngleAxis(-degrees, Vector3.forward))
        {
            onRight = false;
            return true;
        }
        return false;
    }
}
