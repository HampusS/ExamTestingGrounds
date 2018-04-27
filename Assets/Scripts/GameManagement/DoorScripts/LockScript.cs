using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockScript : MonoBehaviour {
    [SerializeField]
    GameObject door;
    [SerializeField]
    GameObject key;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player"
            && door.GetComponent<Door>().haskey == true
            && door.GetComponent<Door>().keys.Contains(key))
        {
            door.GetComponent<Door>().currentKeys++;
            GetComponent<Renderer>().material = key.GetComponent<Renderer>().material;

            key.transform.position = transform.position + (Vector3.up * 0.3f);
            key.GetComponent<MeshRenderer>().enabled = true;
            door.GetComponent<Door>().Open();
            door.GetComponent<Door>().haskey = false;
        }
    }
}
