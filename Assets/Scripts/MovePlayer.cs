using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {
    Vector3 targetPos;


	// Use this for initialization
	void Start () {
        targetPos = new Vector3(-transform.position.x, 1, -transform.position.z);
	}

    private void OnTriggerEnter(Collider other)
    {
        //Check players direction and move him 
    }
}
