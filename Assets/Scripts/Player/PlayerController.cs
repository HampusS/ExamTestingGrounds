using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    List<BaseState> states;
    public BaseState currentState { get; set; }
    public bool Grounded { get; set; }

    Rigidbody rgdBody;
    Vector3 moveAmount;
    Vector3 smoothMove;
    RaycastHit hit;


    void Start()
    {
        //if (Input.GetKeyDown("escape"))
        //    Cursor.lockState = CursorLockMode.None;
        //Cursor.lockState = CursorLockMode.Locked;
        rgdBody = GetComponent<Rigidbody>();
        states = new List<BaseState>();
        states.Add(GetComponent<GroundState>());
        states.Add(GetComponent<WallRun>());
    }

    void Update()
    {
        UpdateGroundCheck();
        if (currentState == null || currentState.Exit())
        {
            currentState = null;
            foreach (BaseState state in states)
            {
                if (state.Enter())
                {
                    currentState = state;
                }
            }
        }
        if (currentState != null)
            currentState.Run();
        if(!Grounded)
        Debug.Log(Grounded);
        if(Grounded)
        {
            SnapToGround();
        }

        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(ray.origin, -transform.up * 1.15f, Color.black);
        Debug.DrawRay(ray.origin, -transform.right, Color.black);
        Debug.DrawRay(ray.origin, transform.right, Color.black);
        Debug.DrawRay(ray.origin, transform.forward, Color.black);

    }

    public void UpdateMoveAmount(float speed, float moveFloatiness)
    {
        Vector3 strafe = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, strafe * speed, ref smoothMove, moveFloatiness);
    }

    public void UpdateGroundCheck()
    {
        Grounded = false;
        if (Physics.Raycast(transform.position, -transform.up * 1.25f, out hit))
        {
            Grounded = true;
        }
    }

    void SnapToGround()
    {
        float surfDist = Vector3.Distance(hit.point, transform.position);
        if (surfDist < GetComponent<CapsuleCollider>().height * 0.6f)
        {
            transform.position += hit.normal * ((GetComponent<CapsuleCollider>().height * 0.5f) - surfDist);
        }
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
