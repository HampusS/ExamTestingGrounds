using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    GameObject camHolder;
    public GameObject grenade;
    public float strength = 500;


    // Use this for initialization
    void Start()
    {
        camHolder = GameObject.Find("CameraHolder");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject clone = Instantiate(grenade, transform.position, transform.rotation);
            Rigidbody rb = clone.GetComponent<Rigidbody>();
            rb.AddForce(camHolder.transform.forward * strength);
        }
    }


}
