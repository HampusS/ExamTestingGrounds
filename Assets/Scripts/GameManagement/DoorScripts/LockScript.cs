using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockScript : MonoBehaviour
{
    [SerializeField]
    GameObject door;
    [SerializeField]
    GameObject key;
    bool unlocked;

    void OnTriggerEnter(Collider collider)
    {
        if (!unlocked && door.GetComponent<Door>().keys.Contains(key))
            if (ThrowLogic.Instance.Thrown && collider.tag == "Disk")
            {
                door.GetComponent<Door>().nrOfKeys++;
                GetComponent<Renderer>().material = key.GetComponent<Renderer>().material;

                key.transform.position = transform.position + (Vector3.up * 0.3f);
                key.GetComponent<MeshRenderer>().enabled = true;
                door.GetComponent<Door>().Open();
                ThrowLogic.Instance.ResetMaterial();
                AudioM.Instance.Play("unlock");
                unlocked = true;
            }
    }
}
