using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {


    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        MouseControl.Instance.LockMouse();
        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
    }
}
