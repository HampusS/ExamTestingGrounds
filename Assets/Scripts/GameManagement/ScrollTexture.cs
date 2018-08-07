using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{

    public float scrollX;
    public float scrollY;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float offestX = Time.time * scrollX;
        float offsetY = Time.time * scrollY;

        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offestX, offsetY);
        if (CanvasManager.Instance != null)
            CanvasManager.Instance.HideGUI();
        if (PlayerController.Instance != null)
            Destroy(PlayerController.Instance.gameObject);
    }
}
