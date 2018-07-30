using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour {
    private static Singleton instance;
    public static Singleton Instance { get { return instance; } }

    void Awake () {
		if(instance!=null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
	}
}
