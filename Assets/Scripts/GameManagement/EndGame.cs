using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EndGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CloseApplication();
	}

    void CloseApplication()
    {
        if (Input.anyKey)
        {
            Debug.Log("quit");
            Application.Quit();
            EditorApplication.isPlaying = false;
        }
    }
}
