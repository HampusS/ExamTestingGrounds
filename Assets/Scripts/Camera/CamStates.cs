using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class State
{
    public delegate void Tilt();
    public Tilt run;

    public delegate bool Entry();
    public Entry enter;

    public State(Tilt run, Entry enter)
    {
        this.enter = enter;
        this.run = run;
    }
}

public class CamStates : MonoBehaviour
{
    [SerializeField]
    float tiltSpeed = 2;
    [SerializeField]
    float tiltDegrees = 20;

    public bool onLeft { get; set; }
    public bool onRight { get; set; }
    public bool onAlign { get; set; }

    public bool onBump { get; set; }

    public bool onShake { get; set; }
    float shakeStrength = 10;
    float shakeLength, shakeTime = 0.25f;

    float angle;
    List<State> states = new List<State>();

    void Start()
    {
        states.Add(new State(AlignCamera, Align));
        states.Add(new State(TiltCameraLeft, Left));
        states.Add(new State(TiltCameraRight, Right));
        states.Add(new State(ShakeCamera, Shake));
        states.Add(new State(BumpCamera, Bump));
    }

    private void Update()
    {
        foreach (State state in states)
        {
            if (state.enter())
                state.run();
        }
    }

    public void ResetCamera()
    {
        onLeft = false;
        onRight = false;
        onAlign = false;
        onShake = false;
        onBump = false;
    }

    bool Left()
    {
        return onLeft;
    }

    void TiltCameraLeft()
    {
        if (transform.localRotation == Quaternion.AngleAxis(tiltDegrees, Vector3.forward))
            onLeft = false;
        else
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.AngleAxis(tiltDegrees, Vector3.forward), Time.deltaTime * tiltSpeed);
    }

    bool Right()
    {
        return onRight;
    }

    void TiltCameraRight()
    {
        if (transform.localRotation == Quaternion.AngleAxis(-tiltDegrees, Vector3.forward))
            onRight = false;
        else
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.AngleAxis(-tiltDegrees, Vector3.forward), Time.deltaTime * tiltSpeed);
    }

    bool Align()
    {
        return onAlign;
    }

    void AlignCamera()
    {
        if (transform.localRotation == Quaternion.AngleAxis(0, Vector3.forward))
            onAlign = false;
        else
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.AngleAxis(0, Vector3.forward), Time.deltaTime * tiltSpeed);
    }

    bool Shake()
    {
        return onShake;
    }

    void ShakeCamera()
    {
        shakeLength += Time.deltaTime;
        if (shakeLength >= shakeTime)
        {
            shakeLength = 0;
            onShake = false;
            onAlign = true;
        }
        else
        {
            angle = Random.Range(-10, 10);
            Debug.Log(angle);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * shakeStrength);
        }
    }

    bool Bump()
    {
        return onBump;
    }

    void BumpCamera()
    {
        shakeLength += Time.deltaTime;
        if (shakeLength >= shakeTime)
        {
            shakeLength = 0;
            onBump = false;
            onAlign = true;
        }
        else
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.AngleAxis(6, Vector3.right), Time.deltaTime * 20);
        }
    }
}
