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

    bool exploded;
    float countdown;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != ("Player"))
        {
            Explode();
            GameObject explosion = Instantiate(effect, transform.position, transform.rotation);
            Destroy(explosion, lifeTime);
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        countdown = lifeTime;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("InvisWallLayer"), LayerMask.NameToLayer("InvisWallLayer"));
    }

    // Update is called once per frame
    void Update()
    {
        if (!exploded)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0)
            {
                Explode();
                GameObject explosion = Instantiate(effect, transform.position, transform.rotation);
                Destroy(explosion, lifeTime);
                Destroy(gameObject);
            }
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
                Rigidbody rb = nearObj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    if (rb.tag == "Enemy")
                    {
                        rb.GetComponent<EnemyController>().Damage(damage);
                    }
                    else
                        rb.AddExplosionForce(force, transform.position, blastRadius);
                }
            }
        }
    }
}
