using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowLogic : MonoBehaviour
{
    public static ThrowLogic Instance;
    public bool Thrown { get; set; }
    public bool Recall { get; set; }
    public bool hasExplosion { get; set; }
    public bool hasPierce { get; set; }

    Transform hand;
    Quaternion rotation;
    [SerializeField]
    GameObject system;
    [SerializeField]
    Transform originsPivot;
    [SerializeField]
    float speed = 30;
    Rigidbody rb;
    Vector3 prevPos;

    float blastRadius = 10;
    float force = 2000;
    public float damage = 1;
    bool exploded;

    Vector3 origScale;
    MeshRenderer myRenderer;
    Material startMaterial;

    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        hand = transform.parent;
        rb = GetComponent<Rigidbody>();
        rotation = transform.localRotation;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("InvisWallLayer"), LayerMask.NameToLayer("InvisWallLayer"), true);
        origScale = transform.localScale;
        hasExplosion = true;
        hasPierce = true;
        myRenderer = GetComponentInChildren<MeshRenderer>();
        startMaterial = myRenderer.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Thrown && other.tag != "Player" && other.gameObject.layer != LayerMask.NameToLayer("InvisWallLayer"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                // if(!hasPierce)
                enemy.Damage(damage);
                enemy.KnockBack(transform.position);
            }
            else 
            {
                rb.velocity = Vector3.zero;
                transform.position = prevPos;
            }
        }
    }

    public void Update()
    {
        if (Thrown)
        {
            prevPos = transform.position;
            if (Recall)
                Retrieve();
        }
        else if (transform.position != originsPivot.position)
            transform.position = originsPivot.position;
        transform.localScale = origScale;
    }

    public void Throw()
    {
        if (!Thrown)
        {
            ActivateDisk(true);
            transform.rotation = originsPivot.rotation;
            transform.parent = null;
            rb.velocity = Camera.main.transform.forward * speed;
            AudioM.Instance.Play("throw");
        }
    }

    public void Retrieve()
    {
        float distance = Vector3.Distance(transform.position, hand.position);
        if (distance <= 0.3f)
        {
            ActivateDisk(false);
            transform.position = originsPivot.position;
            transform.parent = hand;
            transform.localRotation = rotation;
            exploded = false;
        }
        else
        {
            rb.velocity = Vector3.zero;
            float lerp = 8;
            if (distance < 2)
                lerp = 60;
            transform.position = Vector3.Lerp(transform.position, hand.position, Time.deltaTime * lerp);
        }
    }

    void ActivateDisk(bool active)
    {
        Thrown = active;
        rb.isKinematic = !active;
        GetComponent<TrailRenderer>().enabled = active;
    }

    public void Explode()
    {
        if (hasExplosion && !exploded)
        {
            exploded = true;
            system.GetComponent<ParticleSystem>().Play(true);
            AudioM.Instance.Play("boom");
            AudioM.Instance.Play("laser");
            Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
            foreach (Collider nearObj in colliders)
            {
                Rigidbody other = nearObj.GetComponent<Rigidbody>();
                if (other != null)
                {
                    if (other.tag == "Enemy")
                    {
                        EnemyController enemy = other.GetComponent<EnemyController>();
                        enemy.KnockBack(transform.position);
                        enemy.Damage(damage);
                    }
                    else if (other != rb)
                        other.AddExplosionForce(force, transform.position, blastRadius);
                }
            }
        }
    }

    public void SetMaterial(Material other)
    {
        myRenderer.material = other;
        GetComponent<TrailRenderer>().material = other;
    }

    public void ResetMaterial()
    {
        myRenderer.material = startMaterial;
        GetComponent<TrailRenderer>().material = startMaterial;
    }

}
