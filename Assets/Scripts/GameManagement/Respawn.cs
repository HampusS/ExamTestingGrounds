using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class Respawn : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform respawn;
    [SerializeField]
    GameObject deathScreen;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            deathScreen.GetComponent<DeathVisual>().Dead();
            Invoke("RespawnPlayer", 2);
        }
    }
    void RespawnPlayer()
    {
        player.transform.position = respawn.transform.position;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        deathScreen.GetComponent<DeathVisual>().Alive();
    }
}
