using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCollider : MonoBehaviour {
    GameObject gloabaSettings;
    PlayerController player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gloabaSettings = GameObject.FindGameObjectWithTag("SceneHandler");
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            player.AddHealth(100);
            gloabaSettings.GetComponent<SceneHandler>().LoadNext();
        }
    }
}
