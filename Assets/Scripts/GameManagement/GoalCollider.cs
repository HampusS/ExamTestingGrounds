using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCollider : MonoBehaviour {
    GameObject gloabaSettings;
    void Awake()
    {
        gloabaSettings = GameObject.FindGameObjectWithTag("SceneHandler");
    }
    void OnTriggerEnter(Collider collider)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().AddHealth(100);
        gloabaSettings.GetComponent<SceneHandler>().LoadNext();
    }
}
