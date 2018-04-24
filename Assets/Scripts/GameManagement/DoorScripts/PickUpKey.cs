using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpKey : MonoBehaviour {
    [SerializeField]
    GameObject door;

	void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            door.GetComponent<Door>().haskey = true;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            //Destroy(this);
        }
    }
}
