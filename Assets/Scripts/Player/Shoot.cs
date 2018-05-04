using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    GameObject camHolder;
    public GameObject grenade;
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
            GameObject clone = Instantiate(grenade, transform.position, transform.rotation);
            Rigidbody rb = clone.GetComponent<Rigidbody>();
            rb.velocity = Vector3.Project(camHolder.transform.forward * strength, camHolder.transform.forward);
            rb.velocity += Vector3.Project(player.velocity, camHolder.transform.forward);

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
