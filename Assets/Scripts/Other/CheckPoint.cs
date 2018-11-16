using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {
    bool activated;

    private void OnTriggerEnter(Collider other)
    {
        if(!activated && other.tag == "Player")
        {
            PlayerController.Instance.CheckPoint = transform.position;
            activated = true;
        }
    }
}
