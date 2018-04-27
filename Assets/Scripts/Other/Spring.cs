using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour {
    Rigidbody player;
    [SerializeField]
    float impulse = 12;
    [SerializeField]
    Vector3 direction;

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
        player.velocity = new Vector3(player.velocity.x, 0, player.velocity.z);
        player.AddForce(transform.TransformDirection(direction.normalized * impulse), ForceMode.VelocityChange);
    }
}
