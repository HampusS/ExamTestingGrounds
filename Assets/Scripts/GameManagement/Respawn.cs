using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class Respawn : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawn;
    [SerializeField] GameObject deathScreen;

    void OnTriggerEnter(Collider other)
    {
        deathScreen.GetComponent<DeathVisual>().Dead();
        Invoke("RespawnPlayer", 2);
    }
    void RespawnPlayer()
    {
        player.transform.position = respawn.transform.position;
        deathScreen.GetComponent<DeathVisual>().Alive();
    }
}
