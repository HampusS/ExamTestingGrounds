using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneHandler : MonoBehaviour
{
    public GameObject mainMenu;
    int[] order = new int[5];
    int current= -1;
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        order = mainMenu.GetComponent<MainMenu>().order;
    }
    void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            Debug.Log("in loading next");
            LoadNext();
        }
    }
    void LoadNext()
    {
        current++;
        SceneManager.LoadScene(order[current]);
    }
}
