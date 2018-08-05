using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagersSingleton : MonoBehaviour {
    public static ManagersSingleton instance;
	// Use this for initialization
	void Start () {
		if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
	}

}
