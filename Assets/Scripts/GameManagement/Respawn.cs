using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class Respawn : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ShadeController.Instance.SetDeathColor();
            ShadeController.Instance.ColorShade();
            Invoke("RespawnPlayer", 2);
        }
    }

    void RespawnPlayer()
    {
        PlayerController.Instance.transform.position = PlayerController.Instance.CheckPoint;
        PlayerController.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ShadeController.Instance.TransparentShade();
    }
}
