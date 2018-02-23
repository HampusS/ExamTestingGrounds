using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    GroundState ground;

    void Start()
    {
        //if (Input.GetKeyDown("escape"))
        //    Cursor.lockState = CursorLockMode.None;
        //Cursor.lockState = CursorLockMode.Locked;
        ground = GetComponent<GroundState>();
    }

    void Update()
    {


        ground.Run();

        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(ray.origin, -transform.up * 1.25f, Color.black);
        Debug.DrawRay(ray.origin, -transform.right, Color.black);
        Debug.DrawRay(ray.origin, transform.right, Color.black);
        Debug.DrawRay(ray.origin, transform.forward, Color.black);

    }

    //private void FixedUpdate()
    //{
    //    if (rgdBody.velocity.y < 0 || rgdBody.velocity.y > 0 && !Input.GetButton("Jump"))
    //    {
    //        rgdBody.velocity += Vector3.up * Physics.gravity.y * 3 * Time.deltaTime;
    //    }

    //    rgdBody.MovePosition(rgdBody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    //}
}
