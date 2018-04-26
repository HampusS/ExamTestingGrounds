using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseLight : MonoBehaviour
{
    float PULSE_RANGE = 6.0f;
    float PULSE_SPEED = 3.0f;
    float PULSE_MINIMUM = 3.0f;
    Light lightObject;

    // Use this for initialization
    void Start()
    {
        lightObject = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        lightObject.range = PULSE_MINIMUM +
                       Mathf.PingPong(Time.time * PULSE_SPEED,
                                      PULSE_RANGE - PULSE_MINIMUM);
    }
}
