using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPack : MonoBehaviour
{
    PlayerController player;
    AudioM audioM;
    public float Amount;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        audioM = FindObjectOfType<AudioM>();
    }

    public void PickedUp()
    {
        player.AddHealth(Amount);
        audioM.Play("healthpickup");
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PickedUp();
        }
    }
}
