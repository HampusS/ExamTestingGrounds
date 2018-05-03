using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    GameObject camHolder;
    public GameObject grenade;
    public float strength = 500;
    [SerializeField]
    float fireRate = 0.25f;
    float timer;
    Rigidbody player;
    CamStates camState;

    // Use this for initialization
    void Start()
    {
        camHolder = GameObject.Find("CameraHolder");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        camState = Camera.main.GetComponent<CamStates>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //Debug.Log(timer);
        if (timer >= fireRate && Input.GetButtonDown("Fire1"))
        {
            FindObjectOfType<AudioM>().Play("launch");
            timer = 0;
            GameObject clone = Instantiate(grenade, transform.position, transform.rotation);
            Rigidbody rb = clone.GetComponent<Rigidbody>();
            rb.AddForce(camHolder.transform.forward * strength + player.velocity);
            camState.onShake = true;
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
