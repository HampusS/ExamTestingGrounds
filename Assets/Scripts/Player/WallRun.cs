using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    RaycastHit hit;
    Rigidbody rgdBody;
    GroundState groundState;

    // Use this for initialization
    void Start()
    {
        rgdBody = GetComponent<Rigidbody>();
        groundState = GetComponent<GroundState>();
    }

    bool Enter()
    {
        if (Input.GetButtonDown("Jump") && groundState.Grounded)
        {
            if (Physics.Raycast(transform.position, transform.right, out hit, 1) || Physics.Raycast(transform.position, -transform.right, out hit, 1))
            {
                if (hit.transform.tag == "wall")
                {
                    return true;
                }
            }
        }
        return false;
    }

    void Run()
    {
        if (Enter())
        {
            GetComponent<Renderer>().material.color = Color.green;
            transform.position += Vector3.up * (GetComponent<CapsuleCollider>().height * 0.15f);
            rgdBody.AddForce(transform.up * 50);
            rgdBody.useGravity = false;
            StartCoroutine(afterRun());
        }
    }
    IEnumerator afterRun()
    {
        yield return new WaitForSeconds(0.5f);
        Exit();
    }

    void Exit()
    {
        GetComponent<Renderer>().material.color = Color.red;
        rgdBody.useGravity = true;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
    }
}
