using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthControl : MonoBehaviour {
    float currentHealth;
    float maxHealth;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetHealth(float amount)
    {
        currentHealth = amount;
    }

    public void AdjustHealth(float amount)
    {
        currentHealth += amount;
    }

    public bool Alive()
    {
        if (currentHealth <= 0)
            return false;
        return true;
    }

}
