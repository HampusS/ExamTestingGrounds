using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPack : MonoBehaviour
{
    PlayerController player;
    AudioSource sound;
    public float Amount;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        sound = GetComponent<AudioSource>();
    }

    public void PickedUp()
    {
        player.AddHealth(Amount);
        sound.Play();
        GetComponent<Renderer>().enabled = false;
        Destroy(gameObject, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PickedUp();
        }
    }
}
