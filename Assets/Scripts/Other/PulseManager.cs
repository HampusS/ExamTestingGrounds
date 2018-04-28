using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseManager : MonoBehaviour {

    [SerializeField]
    List<Light> lights;

	// Use this for initialization
	void Start () {

	}

    private void OnTriggerStay(Collider other)
    {
        foreach (Light light in lights)
        {
            light.GetComponent<PulseLight>().Pulse();
        }
    }
}
