using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    float time, timer = 1.5f;

    // Update is called once per frame
    void Update()
    {
        CloseApplication();
    }

    void CloseApplication()
    {
        time += Time.deltaTime;
        if (time > timer)
        {
            if (Input.anyKey)
            {
                Debug.Log("quit");
                Application.Quit();
                //EditorApplication.isPlaying = false;
            }
        }
    }
}
