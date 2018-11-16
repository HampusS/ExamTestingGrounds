using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance;

    // Use this for initialization
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowGUI()
    {
        CanvasManager.Instance.ShowGUI();
    }

    public void HideGUI()
    {
        CanvasManager.Instance.HideGUI();
    }

    public void Replay()
    {
        ExitPauseMenu();
        ShadeController.Instance.TriggerLevel();

    }

    public void Resume()
    {
        ExitPauseMenu();
    }

    void ExitPauseMenu()
    {
        ShowGUI();
        gameObject.SetActive(false);
        MouseControl.Instance.LockMouse();
        PlayerController.Instance.LockMovement = false;
        CameraControls.Instance.Disable = false;
    }

    public void Settings()
    {

    }

    public void Home()
    {
        SceneHandler.Instance.LoadMenu();
        if (PlayerController.Instance != null)
            Destroy(PlayerController.Instance.gameObject);
        GameObject obj = GameObject.Find("Managers");
        if (obj != null)
            Destroy(obj);
    }

    public void ExitGame()
    {

    }
}
