using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    float speed = 10;
    [SerializeField]
    float jumpForce = 300;
    [SerializeField]
    float sensX = 5f;
    [SerializeField]
    float sensY = 5f;
    [SerializeField]
    float moveFloatiness = .15f;
    bool grounded;
    bool jump;
    Vector3 moveAmount;
    Vector3 smoothMove;
    Rigidbody rgdBody;

    Transform cam;
    Vector3 camRot;
    float camUpDown;

    // Use this for initialization
    void Start () {
        rgdBody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
        jump = Input.GetButtonDown("Jump");
        if (jump)
        {
            rgdBody.AddForce(transform.up * jumpForce);
        }

        if (rgdBody.velocity.y < 0)
        {
            rgdBody.velocity += Vector3.up * Physics.gravity.y * 3 * Time.deltaTime;
        }
        else if (rgdBody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rgdBody.velocity += Vector3.up * Physics.gravity.y * 4 * Time.deltaTime;
        }

        if (grounded)
            GetComponent<Renderer>().material.color = Color.green;
        else
            GetComponent<Renderer>().material.color = Color.red;


        camUpDown += Input.GetAxis("Mouse Y") * sensY;
        camUpDown = Mathf.Clamp(camUpDown, -75, 75);
        cam.localRotation = Quaternion.AngleAxis(-camUpDown, Vector3.right);

        float camLeftRight = Input.GetAxis("Mouse X") * sensX;
        transform.Rotate(Vector3.up, camLeftRight);

        Vector3 strafe = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, strafe * speed, ref smoothMove, moveFloatiness);


        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;
	}

    private void FixedUpdate()
    {
        //if (rgdBody.velocity.y < 0 || rgdBody.velocity.y > 0 && !Input.GetButton("Jump")) // NOT WORKING PROPERLY  -  UPSIDEDOWN NEEDS INVERTED NUMBERS
        //{
        //    gravityBody.gravityMultiplier = startGravMult * 4;
        //}

        rgdBody.MovePosition(rgdBody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
}
