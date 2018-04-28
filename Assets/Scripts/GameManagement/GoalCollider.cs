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
        gloabaSettings.GetComponent<SceneHandler>().LoadNext();
    }
}
