using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowLogic : MonoBehaviour{

    [SerializeField]
    float speed;
    Rigidbody player;
    
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").
            GetComponent<Rigidbody>();
    }

	public void Update () {
        if (transform.parent != null)
            transform.parent = null;
        else
        {
            //lerp back to player in a cool manner

        }

	}


}
