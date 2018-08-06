using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour
{
    public GameObject projectile;
    public float speed = 20;

    public void Spawn()
    {
        GameObject clone = Instantiate(projectile, transform.position, transform.rotation);
        Rigidbody rb = clone.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

}
