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
        if (collision.gameObject.layer != LayerMask.NameToLayer("InvisWallLayer") && collision.collider.tag != ("Player"))
            Explode();
        else
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("InvisWallLayer"), 0);
    }

    // Use this for initialization
    void Start()
    {
        countdown = lifeTime;
        rb = GetComponent<Rigidbody>();
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
                            rb.velocity = ((rb.position - transform.position).normalized * force) / (blastRadius + Vector3.Distance(rb.position, transform.position));
                            rb.GetComponent<EnemyController>().Damage(damage);
                        }
                        else
                            rb.AddExplosionForce(force, transform.position, blastRadius);
                    }
                }
            }
            GameObject explosion = Instantiate(effect, transform.position, transform.rotation);
            Destroy(explosion, lifeTime);
            GetComponent<Renderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            Destroy(gameObject, lifeTime);
        }
    }
}
