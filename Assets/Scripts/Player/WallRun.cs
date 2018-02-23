using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{

    bool isWallR, isWallL;
    RaycastHit hitR, hitL;
    int jumpCount;
    Rigidbody rgdBody;

    // Use this for initialization
    void Start()
    {
        rgdBody = GetComponent<Rigidbody>();
    }


    void Run()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.Raycast(transform.position, transform.right, out hitR, 1))
            {
                GetComponent<Renderer>().material.color = Color.green;
                if (hitR.transform.tag == "wall")
                {
                    isWallR = true;
                    isWallL = false;
                    rgdBody.useGravity = false;
                }
            }
            else if (Physics.Raycast(transform.position, -transform.right, out hitL, 1))
            {
                if (hitL.transform.tag == "wall")
                {
                    isWallR = false;
                    isWallL = true;
                    rgdBody.useGravity = false;
                }
            }
            StartCoroutine(afterRun());
        }
    }
    IEnumerator afterRun()
    {
        yield return new WaitForSeconds(0.15f);
        isWallR = false;
        isWallL = false;
        rgdBody.useGravity = true;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
    }
}
