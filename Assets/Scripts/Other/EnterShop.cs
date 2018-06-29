using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterShop : MonoBehaviour {

    [SerializeField]
    Transform shopKeeper;
    PlayerController player;
    bool interact;
    CameraControls cam;
	// Use this for initialization
	void Start () {
        GameObject temp = GameObject.FindGameObjectWithTag("Player");
        player = temp.GetComponent<PlayerController>();
        cam = temp.GetComponentInChildren<CameraControls>();
	}
	
	// Update is called once per frame
	void Update () {

    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.E))
        {
            interact = true;
            player.Disable = true;
            cam.Disable = true;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            player.Disable = false;
            cam.Disable = false;
            interact = false;
        }
        if (interact)
        {
            Vector3 target = other.transform.position - shopKeeper.position;
            GameObject temp = GameObject.FindGameObjectWithTag("Player");
            cam.TiltCameraUpDown(-target);
            RotateKeeper(target, shopKeeper);
            RotateKeeper(-target, temp.transform);
        }
    }

    void RotateKeeper(Vector3 target, Transform from)
    {
        float speed = 5 * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(from.forward, target, speed, 0);
        newDir = Vector3.ProjectOnPlane(newDir, Vector3.up);
        from.rotation = Quaternion.LookRotation(newDir);
    }

    private void OnTriggerExit(Collider other)
    {
        interact = false;
    }
}
