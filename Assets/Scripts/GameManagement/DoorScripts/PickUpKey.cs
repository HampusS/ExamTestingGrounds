using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpKey : MonoBehaviour
{
    [SerializeField]
    GameObject door;
    bool pickedUp = false;

    void OnTriggerEnter(Collider collider)
    {
        if (!pickedUp)
            if (ThrowLogic.Instance.Thrown && !ThrowLogic.Instance.Recall && collider.tag == "Disk")
            {
                HideKey();
            }
    }

    public void HideKey()
    {
        pickedUp = true;
        door.GetComponent<Door>().keys.Add(gameObject);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        AudioM.Instance.Play("key");
        ThrowLogic.Instance.SetMaterial(GetComponent<MeshRenderer>().material);
    }
}
