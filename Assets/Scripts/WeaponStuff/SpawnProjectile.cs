using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour
{
    GameObject camHolder;
    public GameObject projectile;
    public float strength = 20;
    [SerializeField]
    float fireRate = 0.25f;
    float timer;
    Rigidbody player;

    void Start()
    {
        camHolder = GameObject.Find("CameraHolder");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= fireRate && Input.GetButtonDown("Fire1"))
        {
            FindObjectOfType<AudioM>().Play("launch");
            timer = 0;
            GameObject clone = Instantiate(projectile, transform.position, transform.rotation);
            Rigidbody rb = clone.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * strength;
            rb.velocity += Vector3.Project(player.velocity, transform.forward);

            Invoke("Reload", fireRate/2);
        }
        else
        {
            //FindObjectOfType<AudioM>().Play("click");
        }
    }

    void Reload()
    {
            FindObjectOfType<AudioM>().Play("reload");
    }


}
