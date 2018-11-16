using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour {
    Rigidbody playerRB;
    [SerializeField]
    float impulse = 12;
    [SerializeField]
    Vector3 direction;

	// Use this for initialization
	void Start () {
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
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
        playerRB.velocity = new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z);
        playerRB.AddForce(transform.TransformDirection(direction.normalized * impulse), ForceMode.Impulse);
        PlayerController.Instance.ForceGravity = true;
    }
}
