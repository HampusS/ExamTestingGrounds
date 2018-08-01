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
            Thorw();
        else
            Retrive();
	}
    void Thorw()
    {
        transform.parent = null;

        transform.Translate(Vector3.forward * Time.deltaTime);
    }
    void Retrive()
    {

    }


}
