using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance;
    bool ended = false;
    int current = 2;
    int menu = 0, hub = 1;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            IncreaseLevelIndex();
            LoadCurrentLevel();
        }

        if (!ended && SceneManager.GetActiveScene().name == "Endingscreen")
        {
            ended = true;
            CanvasManager.Instance.HideGUI();
            PlayerController.Instance.LockMovement = true;
        }
    }

    public void IncreaseLevelIndex()
    {
        current++;
    }

    public void LoadCurrentLevel()
    {
        SceneManager.LoadScene(current);
    }

    public void LoadHub()
    {
        SceneManager.LoadScene(hub);
        MouseControl.Instance.LockMouse();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(menu);
    }
}
