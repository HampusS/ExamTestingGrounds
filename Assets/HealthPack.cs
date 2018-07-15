using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {
    PlayerController player;
    public float Amount;
    bool used;
	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>();	
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!used && other.tag == "Player")
        {
            used = true;
            player.AddHealth(Amount);
            Destroy(gameObject, 0.25f);
        }
    }
}
