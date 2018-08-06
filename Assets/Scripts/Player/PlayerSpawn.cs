using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {
    
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        ShadeController.Instance.SetTeleportColor();
        MouseControl.Instance.LockMouse();
        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
    }
}
