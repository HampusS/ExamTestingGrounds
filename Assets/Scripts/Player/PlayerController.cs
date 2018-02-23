using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    GroundState ground;
    // Use this for initialization
    void Start()
    {
        //if (Input.GetKeyDown("escape"))
        //    Cursor.lockState = CursorLockMode.None;
        //Cursor.lockState = CursorLockMode.Locked;
        ground = GetComponent<GroundState>();
    }

    // Update is called once per frame
    void Update()
    {
        ground.Run();
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
