using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour {
    Rigidbody player;
    [SerializeField]
    float impulse = 12;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            AddImpulse();
        }
    }

    void AddImpulse()
    {
        player.velocity += Vector3.up * impulse;
    }
}
