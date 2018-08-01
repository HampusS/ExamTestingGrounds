using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float lifeTime = 3;
    public float blastRadius = 5;
    public float force = 700;
    public float damage = 1;
    public GameObject effect;
    Rigidbody rb;

    bool exploded;
    float countdown;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != ("Player"))
            Explode();
    }

    // Use this for initialization
    void Start()
    {
        countdown = lifeTime;
        rb = GetComponent<Rigidbody>();
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("InvisWallLayer"), LayerMask.NameToLayer("InvisWallLayer"));
    }

    // Update is called once per frame
    void Update()
    {
        if (!exploded)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0)
                Explode();
        }
    }

    void Explode()
    {
        if (!exploded)
        {
            exploded = true;

            Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
            foreach (Collider nearObj in colliders)
            {
                if (nearObj.tag != "Player")
                {
                    Rigidbody rb = nearObj.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        if (rb.tag == "Enemy")
                        {
                            float dist = Vector3.Distance(rb.position, transform.position);
                            rb.velocity = ((rb.position - transform.position).normalized * force) /*/ blastRadius*/;
                            rb.GetComponent<EnemyController>().Damage(damage);
                        }
                        else
                            rb.AddExplosionForce(force, transform.position, blastRadius);
                    }
                }
            }
            //GameObject explosion = Instantiate(effect, transform.position, transform.rotation);
            //Destroy(explosion, lifeTime);
            Destroy(gameObject);
        }
    }
}
