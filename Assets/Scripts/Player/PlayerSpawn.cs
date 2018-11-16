using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (ShadeController.Instance != null)
            ShadeController.Instance.SetTeleportColor();
        if (MouseControl.Instance != null)
            MouseControl.Instance.LockMouse();
        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
        PlayerController.Instance.CheckPoint = transform.position;
    }
}
