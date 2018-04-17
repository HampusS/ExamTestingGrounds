using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 3;
    public float blastRadius = 5;
    public float force = 700;
    public GameObject effect;

    bool exploded;
    float countdown;

    private void OnCollisionEnter(Collision collision)
    {
        //Instantiate(effect, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (Collider nearObj in colliders)
        {
            Rigidbody rb = nearObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, blastRadius);
            }
        }

        Destroy(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0 && !exploded)
            Explode();
    }

    void Explode()
    {
        exploded = true;
        //Instantiate(effect, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (Collider nearObj in colliders)
        {
            Rigidbody rb = nearObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, blastRadius);
            }
        }

        Destroy(gameObject);
    }
}
