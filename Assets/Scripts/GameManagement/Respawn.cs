using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class Respawn : MonoBehaviour
{
    [SerializeField]
    private Transform respawn;

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
        PlayerController.Instance.transform.position = respawn.transform.position;
        PlayerController.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        PlayerController.Instance.transform.rotation = respawn.rotation;
        ShadeController.Instance.TransparentShade();
    }
}
