using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : MonoBehaviour {
    [SerializeField]
    LayerMask groundedMask;
    [SerializeField]
    float speed = 10;
    [SerializeField]
    float jumpForce = 300;
    [SerializeField]
    float moveFloatiness = .15f;

    bool jump;
    Vector3 moveAmount;
    Vector3 smoothMove;
    Rigidbody rgdBody;
    RaycastHit hit;
    bool Grounded;

    void Start()
    {
        rgdBody = GetComponent<Rigidbody>();
    }

    public void Run()
    {
        Grounded = false;
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out hit, 1.25f, groundedMask))
        {
            Grounded = true;
            float surfDist = Vector3.Distance(hit.point, transform.position);
            if (!jump && surfDist < GetComponent<CapsuleCollider>().height * 0.6f)
            {
                transform.position += hit.normal * ((GetComponent<CapsuleCollider>().height * 0.5f) - surfDist);
            }
        }

        jump = Input.GetButtonDown("Jump");
        if (jump && Grounded)
        {
            transform.position += Vector3.up * (GetComponent<CapsuleCollider>().height * 0.15f);
            rgdBody.AddForce(transform.up * jumpForce);
        }

        if (Grounded)
        {
            Vector3 strafe = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            moveAmount = Vector3.SmoothDamp(moveAmount, strafe * speed, ref smoothMove, moveFloatiness);
        }

        if (Grounded)
            GetComponent<Renderer>().material.color = Color.green;
        else
            GetComponent<Renderer>().material.color = Color.red;
    }

    private void FixedUpdate()
    {
        if (rgdBody.velocity.y < 0 || rgdBody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rgdBody.velocity += Vector3.up * Physics.gravity.y * 3 * Time.deltaTime;
        }

        rgdBody.MovePosition(rgdBody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
}
