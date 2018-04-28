using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseLight : MonoBehaviour
{
    float speed = 3.0f;
    float maximum = 6.0f;
    float minimum = 3.0f;
    Light lightObject;

    // Use this for initialization
    void Start()
    {
        lightObject = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        lightObject.range = minimum + Mathf.PingPong(Time.time * speed, maximum - minimum);
    }
}
