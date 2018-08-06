using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    float time, timer = 1.5f;
    [SerializeField]
    Text fadeInText;
    [SerializeField]
    Color targetColor;
    float speed = 2;
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
            fadeInText.color = Color.Lerp(fadeInText.color, targetColor, Time.deltaTime * speed);
            if (Input.anyKey)
            {
                Application.Quit();
                //EditorApplication.isPlaying = false;
            }
        }
    }
}
