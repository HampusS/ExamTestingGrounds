using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowLogic : MonoBehaviour{

    [SerializeField]
    float speed;
    bool thrown;
    
	void Start () {

    }

	public void Update () {
        if (thrown)
        {
            transform.Translate(Vector3.forward * Time.deltaTime);
        }
    }
    public void Throw()
    {
        Debug.Log(transform + " " + transform.parent);
        Transform temp = transform.parent;
        transform.parent = null;
        transform.position = temp.position;
        thrown = true;
    }
    void Retrive()
    {

    }


}
