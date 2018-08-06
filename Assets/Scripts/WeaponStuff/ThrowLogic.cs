using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowLogic : MonoBehaviour
{
    public static ThrowLogic Instance;
    public bool Thrown { get; set; }
    public bool Recall { get; set; }
    Transform hand;
    Quaternion rotation;
    [SerializeField]
    Transform startPivot;
    [SerializeField]
    float speed = 30;
    Rigidbody rb;
    Vector3 prevPos;

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
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("InvisWallLayer"), LayerMask.NameToLayer("InvisWallLayer"));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Thrown && other.tag != "Player")
        {
            rb.velocity = Vector3.zero;
            transform.position = prevPos;
            EnemyController orb = other.GetComponent<EnemyController>();
            if (orb != null)
            {
                orb.Damage(10);
                orb.KnockBack(transform.position);
            }
        }
    }

    public void Update()
    {
        if (Thrown && Recall)
        {
            Retrieve();
        }
    }

    private void FixedUpdate()
    {
        if (Thrown)
        {
            prevPos = transform.position;
        }
    }

    public void Throw()
    {
        if (!Thrown)
        {
            ActivateDisk(true);
            transform.parent = null;
            rb.velocity = Camera.main.transform.forward * speed;
            transform.rotation = Quaternion.identity;
            FindObjectOfType<AudioM>().Play("throw");
        }
    }

    public void Retrieve()
    {
        float distance = Vector3.Distance(transform.position, hand.position);
        if (distance <= 0.3f)
        {
            ActivateDisk(false);
            transform.position = startPivot.position;
            transform.parent = hand;
            transform.localRotation = rotation;
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

}
