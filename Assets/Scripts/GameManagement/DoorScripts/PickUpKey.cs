using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpKey : MonoBehaviour {
    [SerializeField]
    GameObject door;
    bool pickedUp = false;

	void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && pickedUp == false)
        {
            pickedUp = true;
            door.GetComponent<Door>().keys.Add(gameObject);
            door.GetComponent<Door>().haskey = true;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
