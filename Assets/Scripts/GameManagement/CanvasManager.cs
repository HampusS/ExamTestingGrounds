using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {
    public static CanvasManager Instance;
    public GameObject ShopCanvas;
    public GameObject UpgradesCanvas;
    public GameObject ScreenShade;
    public GameObject Crosshair;
    public Text hpText;
    public Text hpUpText;
    public Image hpImg;
    public FlickerHealth flickerHP { get; private set; }
    public bool GUI { get; set; }

    // Use this for initialization
    void Start () {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        flickerHP = GetComponentInChildren<FlickerHealth>();
	}

    public void HideGUI()
    {
        hpText.gameObject.SetActive(false);
        hpUpText.gameObject.SetActive(false);
        hpImg.gameObject.SetActive(false);
        Crosshair.SetActive(false);
        GUI = false;
    }

    public void ShowGUI()
    {
        hpText.gameObject.SetActive(true);
        hpUpText.gameObject.SetActive(true);
        hpImg.gameObject.SetActive(true);
        Crosshair.SetActive(true);
        GUI = true;
    }

}
